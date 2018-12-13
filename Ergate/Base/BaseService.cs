using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ergate
{
    public class BaseService
    {
        private IConfigurationRoot config;

        public BaseService()
        {
            //读取配置文件
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            config = configBuilder.Build();
        }

        /// <summary>
        /// 主业务数据库名
        /// </summary>
        protected string MasterStoreName
        {
            get
            {
                return GetConnection().Database;
            }
        }

        /// <summary>
        /// 基础数据的库名，用于跨库join查询，与业务数据库在同一实例中
        /// </summary>
        protected string BasicStoreName
        {
            get
            {
                return config.GetSection("ConnectionString:BasicStoreName").Value;
            }
        }

        /// <summary>
        /// 辅助数据库名，作用同 BasicStoreName
        /// </summary>
        [Obsolete("过渡时期使用")]
        protected string AccessoryStoreName
        {
            get
            {
                return config.GetSection("ConnectionString:AccessoryStoreName").Value;
            }
        }

        /// <summary>
        /// 获取业务数据库链接
        /// </summary>
        /// <returns></returns>
        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(config.GetSection("ConnectionString:Database").Value);
        }
    }
}
