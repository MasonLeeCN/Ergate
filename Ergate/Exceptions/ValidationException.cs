using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public class ValidationException : Exception
    {
        public ValidationException(string field, string message)
        {
            this.Field = field;
            this.Message = message;
        }
        /// <summary>
        /// 字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public new string Message { get; set; }
    }
}
