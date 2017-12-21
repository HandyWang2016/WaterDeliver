using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BusinessHelper
{
    public class ProductHelper : MongoBase
    {
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Products>();

                // 从集合中删除指定的对象  
                collection.Remove(x => x.Id == id);
            }
        }

        /// <summary>
        /// 获取所有产品信息
        /// </summary>
        /// <returns></returns>
        public static List<Products> ProductList()
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Products>();
                return collection.FindAll().Documents.ToList();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(Products user)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Products>();

                // 更新对象  
                collection.Update(user, item => item.Id == user.Id);
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Products GetById(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Products>();

                // 查询单个对象  
                return collection.FindOne(x => x.Id == id);
            }
        }
    }
}
