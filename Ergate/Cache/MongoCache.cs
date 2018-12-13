using Ergate.Base;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ergate
{
    /// <summary>
    /// 缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    sealed class MongoCache<T> : BaseService where T : BaseEntity
    {
        public IMongoCollection<T> Table;

        /// <summary>
        /// 数据集
        /// </summary>
        public IMongoQueryable<T> Collection { get; set; }

        /// <summary>
        /// 基础缓存构造函数
        /// </summary>
        public MongoCache()
        {
            //读取配置文件
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            var client = new MongoClient(config.GetSection("ConnectionString:Cache").Value);
            var database = client.GetDatabase("base");
            var name = typeof(T);
            Table = database.GetCollection<T>(name.Name.ToLower());
            Collection = Table.AsQueryable();
        }

        /// <summary>
        /// 指定缓存库构造函数
        /// </summary>
        /// <param name="database"></param>
        public MongoCache(string database)
        {
            //读取配置文件
            var configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = configBuilder.Build();
            var client = new MongoClient(config.GetSection("ConnectionString:Cache").Value);
            var dbbase = client.GetDatabase(database.ToLower());
            var name = typeof(T);
            Table = dbbase.GetCollection<T>(name.Name.ToLower());
            Collection = Table.AsQueryable();
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="model"></param>
        public void Insert(T model)
        {
            Table.InsertOneAsync(model);
        }

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kv"></param>
        public void Modify(string id, List<(string, string)> kvs)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);

            UpdateDefinition<T> updated = null;
            foreach (var kv in kvs)
            {
                updated = Builders<T>.Update.Set(kv.Item1, kv.Item2);
            }

            UpdateResult result = Table.UpdateOneAsync(filter, updated).Result;
        }

        /// <summary>
        /// 更新整个对象
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            var old = Table.Find(e => e.Id.Equals(entity.Id)).ToList().FirstOrDefault();

            foreach (var prop in entity.GetType().GetProperties())
            {
                var newValue = prop.GetValue(entity);
                var oldValue = old.GetType().GetProperty(prop.Name).GetValue(old);
                if (newValue != null)
                {
                    if (!newValue.ToString().Equals(oldValue.ToString()))
                    {
                        old.GetType().GetProperty(prop.Name).SetValue(old, newValue.ToString());
                    }
                }
            }

            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            ReplaceOneResult result = Table.ReplaceOneAsync(filter, old).Result;
        }

        /// <summary>
        /// 删除指定对象
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            Table.DeleteOneAsync(filter);
        }
    }
}
