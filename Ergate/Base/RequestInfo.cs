using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public class RequestInfo
    {
        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        /// <value>
        /// The server URL.
        /// </value>
        public static string ServerUrl { get; set; }

        /// <summary>
        /// 当前url(去除?后面的参数)
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// ?后面参数
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Form 信息
        /// </summary>
        public string FormData { get; set; }

        /// <summary>
        /// header 信息
        /// </summary>
        public string Header { get; set; }

        public Exception Exception { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
