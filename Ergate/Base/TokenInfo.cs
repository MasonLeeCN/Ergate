using System;
using System.Collections.Generic;
using System.Text;

namespace Ergate
{
    public class TokenInfo
    {
        public string unique_name { get; set; }
        public string nameid { get; set; }
        public string jti { get; set; }
        public string nbf { get; set; }
        public string exp { get; set; }
        public string iat { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }
}
