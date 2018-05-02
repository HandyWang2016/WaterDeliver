using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.BusinessHelper;

namespace WaterDeliver.Controllers
{
    /// <summary>
    /// 产品月送水成本汇总
    /// </summary>
    public class Home_ProCostController : BaseController
    {
        // GET: Home_ProCost
        public ActionResult Index(string productId, string yearMonth)
        {
            var products = ProductHelper.ProductList();
            var dailyRecords = DailyRecordHelper.DailyRecordList();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.AddMonths(-1).Month;//默认查上一个月

            if (!string.IsNullOrEmpty(productId))
            {
                dailyRecords = dailyRecords.Where(item => item.SendProductId == productId).ToList();
                ViewBag.productId = productId;
            }
            if (!string.IsNullOrEmpty(yearMonth))
            {
                year = int.Parse(yearMonth.Split('-')[0]);
                month = int.Parse(yearMonth.Split('-')[1]);
            }
            ViewBag.yearMonth = year + "-" + month;

            List<ProductCostModel> proCosts = (from d in dailyRecords
                                               join p in products
                                                   on d.SendProductId equals p.Id
                                               where d.VisitDate.Year == year && d.VisitDate.Month == month
                                               select new ProductCostModel()
                                               {
                                                   ProductName = p.ProductName,
                                                   SumCost = d.SendBucketAmount * p.CostPrice,
                                                   YearMonth = year + "-" + month
                                               }).ToList();
            proCosts = proCosts.GroupBy(item => item.ProductName).Select(item => new ProductCostModel()
            {
                ProductName = item.First().ProductName,
                YearMonth = item.First().YearMonth,
                SumCost = item.Sum(i => i.SumCost)
            }).ToList();
            ViewBag.Products = products;
            ViewBag.flag = "ProductCost";

            return View(proCosts);
        }

        /// <summary>
        /// 根据产品/年月 查询
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="yearMonth"></param>
        /// <returns></returns>
        public ActionResult QueryPro(string productId, string yearMonth)
        {
            return RedirectToAction("Index", new { productId, yearMonth });
        }
    }


    public class ProductCostModel
    {
        public string ProductName { get; set; }
        public string YearMonth { get; set; }
        public double SumCost { get; set; }
    }
}