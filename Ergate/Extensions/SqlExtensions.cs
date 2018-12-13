using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    /// <summary>
    /// 用于sql语句的扩展
    /// </summary>
    public static class SqlExtensions
    {
        /// <summary>
        /// int数组,生成sql
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToSqlString(this List<int> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var i in list)
                {
                    builder.Append($"{i},");
                }
                return builder.ToString().TrimEnd(',');
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// int数组,生成sql
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ToSqlString(this List<string> list)
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var i in list)
                {
                    builder.Append($"'{i}',");
                }
                return builder.ToString().TrimEnd(',');
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ToSqlString<T>(this List<T> list) where T : Enum
        {
            if (list != null && list.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var i in list)
                {
                    builder.Append($"{i},");
                }
                return builder.ToString().TrimEnd(',');
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取线程安全的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ConcurrentBag<T> ToThreadSafeList<T>(this IList<T> list) where T : class
        {
            var items = new ConcurrentBag<T>();
            foreach (T t in list)
            {
                items.Add(t);
            }
            return items;
        }

        /// <summary>
        /// 获取线程安全的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ConcurrentBag<T> ToThreadSafeList<T>(this IEnumerable<T> list) where T : class
        {
            var items = new ConcurrentBag<T>();
            foreach (T t in list)
            {
                items.Add(t);
            }
            return items;
        }
    }
}
