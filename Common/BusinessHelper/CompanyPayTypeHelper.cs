using Model;
using MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.BusinessHelper
{
    public class CompanyPayTypeHelper : MongoBase
    {
        /// <summary>
        /// 删除支付类别信息
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
                    var collection = db.GetCollection<CompanyPayType>();

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
        /// 获取所有支付类别
        /// </summary>
        /// <returns></returns>
        public static List<CompanyPayType> PayTypeList()
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<CompanyPayType>();
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
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CompanyPayType GetById(string id)
        {
            try
            {
                using (Mongo mongo = new Mongo(_connectionString))
                {
                    mongo.Connect();
                    var db = mongo.GetDatabase(_dbName);
                    var collection = db.GetCollection<CompanyPayType>();

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