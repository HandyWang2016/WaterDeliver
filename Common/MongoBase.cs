using log4net;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log4net.config", Watch = true)]
namespace Common
{
    public class MongoBase
    {
        protected static readonly ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static string _connectionString = ConfigurationManager.AppSettings["DataSerer"];
        protected static string _dbName = ConfigurationManager.AppSettings["DataBase"];

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mod"></param>
        public static void Insert<T>(T mod) where T : class
        {
            try
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
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mods"></param>
        public static void Insert<T>(List<T> mods) where T : class
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    //打开连接
                    mongo.Connect();
                    //数据操作对象
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<T>();
                    foreach (var mod in mods)
                    {
                        collection.Insert(mod);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }
    }
}