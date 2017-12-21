using MongoDB;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class MongoBase
    {
        protected static string _connectionString = ConfigurationManager.AppSettings["DataSerer"];
        protected static string _dbName = ConfigurationManager.AppSettings["DataBase"];

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mod"></param>
        public static void Insert<T>(T mod) where T : class
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                //打开连接
                mongo.Connect();
                //数据操作对象
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<T>();
                collection.Insert(mod);
            }
        }
    }
}