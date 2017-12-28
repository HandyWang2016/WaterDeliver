using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BusinessHelper
{
    public class DailyRecordHelper : MongoBase
    {
        /// <summary>
        /// 删除日常纪录信息
        /// </summary>
        /// <param name="id"></param>
        public static void Delete(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<DailyRecord>();

                // 从集合中删除指定的对象  
                collection.Remove(x => x.Id == id);
            }
        }

        /// <summary>
        /// 获取所有日常纪录信息
        /// </summary>
        /// <returns></returns>
        public static List<DailyRecord> DailyRecordList()
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<DailyRecord>();
                return collection.FindAll().Documents.ToList();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(DailyRecord user)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<DailyRecord>();

                // 更新对象  
                collection.Update(user, item => item.Id == user.Id);
            }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DailyRecord GetById(string id)
        {
            using (Mongo mongo = new Mongo(_connectionString))
            {
                mongo.Connect();
                var db = mongo.GetDatabase(_dbName);
                var collection = db.GetCollection<DailyRecord>();

                // 查询单个对象  
                return collection.FindOne(x => x.Id == id);
            }
        }
    }
}
