using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.BusinessHelper
{
    public class StaffCustomerHelper:MongoBase
    {
        /// <summary>
        /// 删除员工客户信息
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
                    var collection = db.GetCollection<StaffCustomer>();

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
        /// 获取所有员工客户信息
        /// </summary>
        /// <returns></returns>
        public static List<StaffCustomer> StaffCustomerList()
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffCustomer>();
                    return collection.FindAll().Documents.ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new List<StaffCustomer>();
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="user"></param>
        public static void Update(StaffCustomer user)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffCustomer>();

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
        public static StaffCustomer GetById(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffCustomer>();

                    // 查询单个对象  
                    return collection.FindOne(x => x.Id == id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return new StaffCustomer();
            }

        }
    }
}