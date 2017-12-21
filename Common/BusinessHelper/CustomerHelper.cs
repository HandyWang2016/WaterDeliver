using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using Model;

namespace Common.BusinessHelper
{
    public class CustomerHelper : MongoBase
    {
        /// <summary>
        /// 删除客户信息
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Customer>();

                // 从集合中删除指定的对象  
                collection.Remove(x => x.Id == id);
            }
        }

        /// <summary>
        /// 获取所有客户信息
        /// </summary>
        /// <returns></returns>
        public static List<Customer> CustomerList()
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Customer>();
                return collection as List<Customer>;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(Customer user)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Customer>();

                // 更新对象  
                collection.Update(user, item => item.Id == user.Id);
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer GetById(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<Customer>();

                // 查询单个对象  
                return collection.FindOne(x => x.Id == id);
            }
        }
    }
}
