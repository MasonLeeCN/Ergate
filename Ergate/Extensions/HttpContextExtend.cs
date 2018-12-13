using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    /// <summary>
    /// extend httpcontext
    /// </summary>
    public static class HttpContextExtend
    {
        /// <summary>
        /// Gets the query string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetQueryStr(this HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine();
                var index = 0;
                foreach (var key in request.Query.Keys)
                {
                    if (index == 0)
                    {
                        sb.Append($"?{key}={request.Query[key]}");
                    }
                    else
                    {
                        sb.Append($"&{key}={request.Query[key]}");
                    }
                    index++;
                }
            }
            catch (Exception)
            {

            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the form string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetFormStr(this HttpRequest request)

        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine();
                foreach (var item in request.Form)
                {
                    sb.Append($"{item.Key} : {item.Value}\r\n");
                }
            }
            catch (Exception)
            {

            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the header string.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public static string GetHeaderStr(this HttpRequest request)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (var item in request.Headers)
                {
                    sb.Append($"{item.Key} : {item.Value}\r\n");
                }
            }
            catch (Exception)
            {

            }
            return sb.ToString();
        }
    }
}
