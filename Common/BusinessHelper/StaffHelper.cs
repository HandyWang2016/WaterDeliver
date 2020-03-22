using log4net;
using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.BusinessHelper
{
    public class StaffHelper : MongoBase
    {
        /// <summary>
        /// 删除员工信息
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
                    var collection = db.GetCollection<Staff>();

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
        /// 获取所有员工信息
        /// </summary>
        /// <returns></returns>
        public static List<Staff> StaffList()
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Staff>();
                    return collection.FindAll().Documents.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new List<Staff>();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(Staff user)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Staff>();

                    // 更新对象  
                    collection.Update(user, item => item.Id == user.Id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Staff GetById(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<Staff>();

                    // 查询单个对象  
                    return collection.FindOne(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new Staff();
            }

        }
    }
}
