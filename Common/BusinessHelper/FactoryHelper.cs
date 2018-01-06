using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BusinessHelper
{
    public class FactoryHelper : MongoBase
    {
        /// <summary>
        /// 删除工厂信息
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Factory>();

                    // 从集合中删除指定的对象  
                    collection.Remove(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        /// <summary>
        /// 获取所有工厂信息
        /// </summary>
        /// <returns></returns>
        public static List<Factory> FactoryList()
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Factory>();
                    return collection.FindAll().Documents.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(Factory factory)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Factory>();

                    // 更新对象  
                    collection.Update(factory, item => item.Id == factory.Id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }

        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Factory GetById(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Factory>();

                    // 查询单个对象  
                    return collection.FindOne(x => x.Id == id);
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
