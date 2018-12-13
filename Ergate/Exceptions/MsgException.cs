using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public class MsgException : Exception
    {
        public int Code { get; set; } = 200;
        public new string Message { get; set; } = "";

        public MsgException(string message) : this(200, message)
        {
            Message = message;
        }

        public MsgException(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
    }
}
