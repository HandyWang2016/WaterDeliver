using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.BusinessHelper;
using Model;
using Newtonsoft.Json;

namespace WaterDeliver.Controllers
{
    public class Home_FactoryController : BaseController
    {
        /// <summary>
        /// 1. 水厂库存明细，统计字段：交易年月 产品名称 库存 空桶库存
        /// 2. 水厂交易明细 支付总押金 退还总押金 进水总支出
        /// </summary>
        /// <returns></returns>
        public ActionResult TransRecord(string yearMonth, string factoryId)
        {
            int year;
            int month;
            if (string.IsNullOrEmpty(yearMonth))
            {
                //默认显示上一个月的月底结算情况
                year = DateTime.Now.AddMonths(-1).Year;
                month = DateTime.Now.AddMonths(-1).Month;
                yearMonth = year + "-" + month;
            }
            else
            {
                year = int.Parse(yearMonth.Split('-')[0]);
                month = int.Parse(yearMonth.Split('-')[1]);
            }
            var factories = FactoryHelper.FactoryList();
            var products = ProductHelper.ProductList();
            var payTypes = CompanyPayTypeHelper.PayTypeList();
            //选定年月下的日常、公司交易记录
            var companyRecords = CompanyRecordHelper.CompanyList();
            var dailyRecords = DailyRecordHelper.DailyRecordList();
            //1.库存明细
            var factoryStocks = (from r in dailyRecords
                                 join p in products
                                 on r.SendProductId equals p.Id
                                 where r.VisitDate.Year == year && r.VisitDate.Month == month
                                 join f in factories
                                 on p.FactoryId equals f.Id
                                 where f.Id == (string.IsNullOrEmpty(factoryId) ? f.Id : factoryId)
                                 group
                                 new { p.FactoryId, p.Id, r.SendBucketAmount, r.ReceiveEmptyBucketAmount, p.ProductName, f.FactoryName } by
                                 new { p.FactoryId, p.Id }
                into t
                                 select new FactoryStock
                                 {
                                     FactoryName = t.First().FactoryName,
                                     TransMonth = yearMonth,
                                     ProductName = t.First().ProductName,
                                     BucketStock = t.Sum(i => i.SendBucketAmount),
                                     EmptyBucketStock = t.Sum(i => i.ReceiveEmptyBucketAmount)
                                 }).OrderByDescending(item => item.FactoryName);

            //2.交易明细
            var factoryTrans = (from c in companyRecords
                                join f in factories
                                on c.FactoryId equals f.Id
                                where c.TransTime.Year == year && c.TransTime.Month == month && f.Id == (string.IsNullOrEmpty(factoryId) ? f.Id : factoryId)
                                join p in payTypes
                                on c.PayTypeId equals p.Id
                                group new { c.IsPayType, c.TransSum, f.FactoryName, p.PayType } by
                                new { c.FactoryId, c.PayTypeId }
                into t
                                select new FactoryTrans
                                {
                                    FactoryName = t.First().FactoryName,
                                    TransType = t.First().PayType,
                                    IsPayType = t.First().IsPayType,
                                    TransSum = t.Sum(i => i.TransSum),
                                    TransMonth = yearMonth
                                }).OrderByDescending(item => item.FactoryName);

            FactorySumaryViewModel factorySumary = new FactorySumaryViewModel
            {
                FactoryStock = factoryStocks.ToList(),
                FactoryTrans = factoryTrans.ToList()
            };

            ViewBag.queryPam = JsonConvert.SerializeObject(new { YearMonth = yearMonth, FactoryId = factoryId });
            factories.Insert(0, new Factory { Id = "", FactoryName = "" });
            ViewBag.factories = factories;
            ViewBag.flag = "FactoryTransRecord";
            ViewBag.YearMonth = yearMonth;
            return View(factorySumary);
        }
    }
}