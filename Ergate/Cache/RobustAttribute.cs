using Dora.Interception;
using System;

namespace Ergate
{
    [AttributeUsage(AttributeTargets.Method)]
    sealed class RobustAttribute : InterceptorAttribute
    {
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
        /// 默认构造函数
        /// </summary>
        public RobustAttribute()
        {
        }

        public override void Use(IInterceptorChainBuilder builder)
        {
            builder.Use<Ergate.Cache.CacheInterceptor>(this.Order,
                this.MaxRetryCount,
                this.RetryIntervalMilliseconds,
                this.IsEnableBreaker,
                this.ExceptionsAllowedBeforeBreaking,
                this.MillisecondsOfBreak,
                this.TimeOutMilliseconds,
                this.CacheTTLMilliseconds);
        }
    }
}
