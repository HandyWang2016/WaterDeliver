using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.BusinessHelper
{
    public class SalaryHelper : MongoBase
    {
        /// <summary>
        /// 删除工资信息
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
                    var collection = db.GetCollection<StaffSalary>();

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
        /// 获取所有工资信息
        /// </summary>
        /// <returns></returns>
        public static List<StaffSalary> SalaryList()
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffSalary>();
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
        public static void Update(StaffSalary salary)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffSalary>();

                    // 更新对象  
                    collection.Update(salary, item => item.Id == salary.Id);
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
        public static StaffSalary GetById(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffSalary>();

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

        /// <summary>
        /// 检查该月份该员工是否已经发过工资
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        public static StaffSalary CheckIfPaid(string staffId, DateTime salaryMonth)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<StaffSalary>();

                    // 查询单个对象  
                    return collection.FindOne(x => x.StaffId == staffId && x.SalaryMonth == salaryMonth);
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