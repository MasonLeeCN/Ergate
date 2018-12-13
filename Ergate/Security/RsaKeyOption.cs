using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ergate.Base
{
    /// <summary>
    /// RSA公私钥配置
    /// </summary>
    public class RsaKeyOption
    {
        private string _publicKey;
        private string _privateKey;

        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 公钥
        /// </summary>
        public string PublicKey
        {
            get
            {
                if (_publicKey.Length > 100)
                {
                    return _publicKey;
                }

                dynamic type = this.GetType();
                string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
                var path = Path.Combine(currentDirectory, _publicKey);

                using (var reader = new StreamReader(path))
                {
                    var output = reader.ReadToEnd();
                    _publicKey = output;
                }

                return _publicKey;
            }
            set { _publicKey = value; }
        }

        /// <summary>
        /// 私钥
        /// </summary>
        public string PrivateKey
        {
            get
            {
                if (_privateKey.Length > 100)
                {
                    return _privateKey;
                }

                dynamic type = this.GetType();
                string currentDirectory = Path.GetDirectoryName(type.Assembly.Location);
                var path = Path.Combine(currentDirectory, _privateKey);

                using (var reader = new StreamReader(path))
                {
                    var output = reader.ReadToEnd();
                    _privateKey = output;
                }

                return _privateKey;
            }
            set { _privateKey = value; }
        }
    }
}
