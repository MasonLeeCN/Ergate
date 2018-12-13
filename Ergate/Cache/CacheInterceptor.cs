using Dora.DynamicProxy;
using Polly;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ergate.Cache
{
    sealed class CacheInterceptor
    {
        private readonly InterceptDelegate _next;

        /// <summary>
        /// 最多重试几次，如果为0则不重试
        /// </summary>
        public int MaxRetryCount { get; set; } = 0;

        /// <summary>
        /// 重试间隔的毫秒数
        /// </summary>
        public int RetryIntervalMilliseconds { get; set; } = 100;

        /// <summary>
        /// 是否启用熔断
        /// </summary>
        public bool IsEnableBreaker { get; set; } = false;

        /// <summary>
        /// 熔断前出现允许错误几次
        /// </summary>
        public int ExceptionsAllowedBeforeBreaking { get; set; } = 3;

        /// <summary>
        /// 熔断多长时间（毫秒）
        /// </summary>
        public int MillisecondsOfBreak { get; set; } = 1000;

        /// <summary>
        /// 执行超过多少毫秒则认为超时（0表示不检测超时）
        /// </summary>
        public int TimeOutMilliseconds { get; set; } = 0;

        /// <summary>
        /// 缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key
        /// </summary>
        public int CacheTTLMilliseconds { get; set; } = 0;

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="next">拦截委托</param>
        /// <param name="maxRetryCount">最多重试几次</param>
        /// <param name="retryIntervalMilliseconds">重试间隔时长，单位毫秒</param>
        /// <param name="isEnableBreaker">是否启用熔断</param>
        /// <param name="exceptionsAllowedBeforeBreaking">熔断前允许异常几次</param>
        /// <param name="millisecondsOfBreak">熔断多长时间</param>
        /// <param name="timeOutMilliseconds">执行超过多少毫秒则认为超时（0表示不检测超时）</param>
        /// <param name="cacheTTLMilliseconds">缓存多少毫秒（0表示不缓存），用“类名+方法名+所有参数ToString拼接”做缓存Key</param>
        public CacheInterceptor(InterceptDelegate next,
            int maxRetryCount,
            int retryIntervalMilliseconds,
            bool isEnableBreaker,
            int exceptionsAllowedBeforeBreaking,
            int millisecondsOfBreak,
            int timeOutMilliseconds,
            int cacheTTLMilliseconds)
        {
            _next = next;
            this.MaxRetryCount = maxRetryCount;
            this.RetryIntervalMilliseconds = retryIntervalMilliseconds;
            this.IsEnableBreaker = isEnableBreaker;
            this.ExceptionsAllowedBeforeBreaking = exceptionsAllowedBeforeBreaking;
            this.MillisecondsOfBreak = millisecondsOfBreak;
            this.TimeOutMilliseconds = timeOutMilliseconds;
            this.CacheTTLMilliseconds = cacheTTLMilliseconds;
        }

        private static ConcurrentDictionary<MethodBase, Policy> policies = new ConcurrentDictionary<MethodBase, Policy>();

        public async Task InvokeAsync(InvocationContext context)
        {
            Console.WriteLine("11");
            policies.TryGetValue(context.Method, out Policy policy);

            lock (policies)
            {
                if (policy == null)
                {
                    //创建一个空的Policy
                    policy = Policy.NoOpAsync();

                    //启用熔断
                    if (IsEnableBreaker)
                    {
                        policy = policy.WrapAsync(
                            Policy.Handle<Exception>()
                            .CircuitBreakerAsync(ExceptionsAllowedBeforeBreaking, TimeSpan.FromMilliseconds(MillisecondsOfBreak)));
                    }
                    //启用超时
                    if (TimeOutMilliseconds > 0)
                    {
                        policy = policy.WrapAsync(
                            Policy.TimeoutAsync(
                                () => TimeSpan.FromMilliseconds(TimeOutMilliseconds), Polly.Timeout.TimeoutStrategy.Pessimistic));
                    }
                    //启用重试
                    if (MaxRetryCount > 0)
                    {
                        policy = policy.WrapAsync(
                            Policy.Handle<Exception>()
                            .WaitAndRetryAsync(MaxRetryCount, i => TimeSpan.FromMilliseconds(RetryIntervalMilliseconds)));
                    }

                    //放入
                    policies.TryAdd(context.Method, policy);
                }
            }

            Context pollyCtx = new Context();
            pollyCtx["aspectContext"] = context;

            //启用缓存
            if (CacheTTLMilliseconds > 0)
            {
                //用类名+方法名+参数的下划线连接起来作为缓存key
                string cacheKey = "Cache_Key_" + context.Method.DeclaringType + "." + context.Method + string.Join("_", context.Arguments);

                RedisCache cache = new RedisCache();
                var value = cache.Get(cacheKey);
                if (!string.IsNullOrEmpty(value))
                {
                    context.ReturnValue = value;
                }
                else
                {
                    //如果缓存中没有，则执行实际被拦截的方法
                    await policy.ExecuteAsync(ctx => _next(context), pollyCtx);

                    var tp = new TimeSpan(0, 0, 0, 0, CacheTTLMilliseconds);
                    var node = new CacheNode<string> { Key = cacheKey, Data = context.ReturnValue.ToObjString(), CacheTime = tp };
                    cache.Set(node);
                }
            }
            else//如果没有启用缓存，就直接执行业务方法
            {
                await policy.ExecuteAsync(ctx => _next(context), pollyCtx);
            }
        }
    }
}
