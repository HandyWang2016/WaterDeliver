using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;
using System.Collections;
using Newtonsoft.Json;

namespace WaterDeliver.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var staffs = StaffHelper.StaffList();
            return View(staffs);
        }

        /// <summary>
        /// 日常记账
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyWrite()
        {
            //所有产品
            List<Products> products = ProductHelper.ProductList();
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            CustomerProductViewModel cusProduct = new CustomerProductViewModel()
            {
                Customers = customers,
                Products = products
            };
            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.flag = "DailyWrite";
            return View(cusProduct);
        }

        /// <summary>
        /// 添加日常记账
        /// </summary>
        /// <param name="dailyRecord"></param>
        /// <returns></returns>
        public ActionResult CreateDailyWrite(DailyRecord dailyRecord)
        {
            dailyRecord.Id = ObjectId.NewObjectId().ToString();
            dailyRecord.VisitDate = dailyRecord.VisitDate < Convert.ToDateTime("2017-01-01") ? DateTime.Now : dailyRecord.VisitDate;

            //更新库存信息
            var product = ProductHelper.GetById(dailyRecord.SendProductId);
            product.StockRemain -= dailyRecord.SendBucketAmount;//桶装水库存减少
            product.BucketStockRemain += dailyRecord.ReceiveEmptyBucketAmount;//空桶库存增加
            ProductHelper.Update(product);

            MongoBase.Insert(dailyRecord);
            return RedirectToAction("DailyRecord");
        }

        /// <summary>
        /// 日常记账记录查看
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyRecord()
        {
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            //所有产品
            var products = ProductHelper.ProductList();
            //所有日常记录
            var dailyRecords = DailyRecordHelper.DailyRecordList();
            //添加空选项
            customers.Insert(0, new Customer { CustomerName = "" });
            //获取页条数
            int pageSize = PageSize();

            List<DailyRecord> currentRecords = TempData["DailyRecord"] == null
                ? dailyRecords
                .OrderByDescending(item => item.VisitDate)
                .Skip(0)
                .Take(pageSize)
                .ToList()
                : TempData["DailyRecord"] as List<DailyRecord>;
            List<DailyRecordShow> newRecords = (from r in currentRecords
                                                join c in customers
                                                on r.CustomerId equals c.Id
                                                join p in products
                                                on r.SendProductId equals p.Id
                                                select new DailyRecordShow
                                                {
                                                    CustomerName = c.CustomerName,
                                                    ProductName = p.ProductName,
                                                    SendBucketAmount = r.SendBucketAmount,
                                                    ReceiveEmptyBucketAmount = r.ReceiveEmptyBucketAmount,
                                                    EarnDeposit = r.EarnDeposit,
                                                    PayDeposit = r.PayDeposit,
                                                    EarnMonthEndPrice = r.EarnMonthEndPrice,
                                                    VisitDate = r.VisitDate

                                                }).ToList();

            ViewBag.flag = "DailyRecord";
            ViewBag.customers = customers;
            ViewBag.queryPam = TempData["dailyQuery"] == null ? "{}" : JsonConvert.SerializeObject(TempData["dailyQuery"]);

            ViewBag.totalPage = TempData["totalPage"] == null
                ? (dailyRecords.Count() % pageSize == 0
                    ? dailyRecords.Count() / pageSize
                    : Math.Ceiling(Convert.ToDouble(dailyRecords.Count()) / pageSize))
                : int.Parse(TempData["totalPage"].ToString());

            ViewBag.totalSize = TempData["totalSize"] == null
                ? dailyRecords.Count
                : int.Parse(TempData["totalSize"].ToString());
            ViewBag.currentPage = TempData["currentPage"] == null
                 ? 1
                 : int.Parse(TempData["currentPage"].ToString());

            ViewBag.Staffs = StaffHelper.StaffList();
            return View(newRecords);
        }

        /// <summary>
        /// 查询日常记录
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult QueryDailyRecord(DailyQueryMod dailyQuery, int pageIndex)
        {
            var records = FilterRecordInfo(dailyQuery, pageIndex);
            TempData["DailyRecord"] = records;
            TempData["dailyQuery"] = dailyQuery;
            return RedirectToAction("DailyRecord");
        }

        /// <summary>
        /// 根据查询条件过滤数据
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <returns></returns>
        private List<DailyRecord> FilterRecordInfo(DailyQueryMod dailyQuery, int pageIndex)
        {
            List<DailyRecord> records = DailyRecordHelper.DailyRecordList();
            IEnumerable<DailyRecord> temRecords = records.Where(item => item.StaffId != "");
            if (!string.IsNullOrEmpty(dailyQuery.StaffId))
            {
                temRecords = temRecords.Where(item => item.StaffId == dailyQuery.StaffId);
            }
            if (!string.IsNullOrEmpty(dailyQuery.CustomerId))
            {
                temRecords = temRecords.Where(item => item.CustomerId == dailyQuery.CustomerId);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2017-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate >= dailyQuery.DateBegin);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2017-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate <= dailyQuery.DateEnd);
            }

            //获取页条数
            int pageSize = PageSize();
            var currentRecords = temRecords
                 .OrderByDescending(item => item.VisitDate)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize);
            //记录分页相关字段
            TempData["totalPage"] = temRecords.Count() % pageSize == 0
                ? temRecords.Count() / pageSize
                : Math.Ceiling(Convert.ToDouble(temRecords.Count()) / pageSize);
            TempData["totalSize"] = temRecords.Count();
            TempData["currentPage"] = pageIndex;

            return currentRecords.ToList<DailyRecord>();
        }

        /// <summary>
        /// 月底结算
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthEnd(string yearMonth, string staffId, int pageSize = 1)
        {
            var records = DailyRecordHelper.DailyRecordList();
            var customers = CustomerHelper.CustomerList();
            if (!string.IsNullOrEmpty(yearMonth))
            {
                int year = int.Parse(yearMonth.Split('-')[0]);
                int month = int.Parse(yearMonth.Split('-')[1]);
                records = records.Where(item => item.VisitDate.Year == year && item.VisitDate.Month == month).ToList();
            }
            if (!string.IsNullOrEmpty(staffId))
            {
                records = records.Where(item => item.StaffId == staffId.ToString()).ToList();
            }
            //按月份、公司分组
            List<SumDailyRecordByCP> sumRecordsByCP = records.OrderByDescending(i => i.VisitDate)
                .GroupBy(item => new { item.VisitDate.Year, item.VisitDate.Month, item.CustomerId })
                .Select(g => new SumDailyRecordByCP
                {
                    SumSendBucketAmount = g.Sum(x => x.SendBucketAmount), //送水桶数
                    SumReceiveEmptyBucketAmount = g.Sum(x => x.ReceiveEmptyBucketAmount), //收回空桶数
                    SumEarnDeposit = g.Sum(x => x.EarnDeposit), //收取押金
                    SumPayDeposit = g.Sum(x => x.PayDeposit), //退还押金
                    SumEarnMonthEndPrice = g.Sum(x => x.EarnMonthEndPrice), //收入月底结算
                    VisitMonth = g.First().VisitDate.Month + "月", //交易月份
                    CustomerName = g.First().CustomerId,
                    ProductName = g.First().SendProductId
                }).Join(customers, x => x.CustomerName, y => y.Id, (x, y) => new { x, y }).Select(p => new SumDailyRecordByCP()
                {
                    CustomerName = p.y.CustomerName,
                    SumSendBucketAmount = p.x.SumSendBucketAmount,
                    SumReceiveEmptyBucketAmount = p.x.SumReceiveEmptyBucketAmount,
                    SumEarnDeposit = p.x.SumEarnDeposit,
                    SumPayDeposit = p.x.SumPayDeposit,
                    SumEarnMonthEndPrice = p.x.SumEarnMonthEndPrice,
                    SumEarnWaterCardPrice = p.x.SumEarnWaterCardPrice,
                    VisitMonth = p.x.VisitMonth
                }).ToList();


            //按月份分组
            List<SumDailyRecord> sumRecords = records.OrderByDescending(i => i.VisitDate)
                .GroupBy(item => new { item.VisitDate.Year, item.VisitDate.Month })
                .Select(g => new SumDailyRecord
                {
                    SumSendBucketAmount = g.Sum(x => x.SendBucketAmount),//送水桶数
                    SumReceiveEmptyBucketAmount = g.Sum(x => x.ReceiveEmptyBucketAmount),//收回空桶数
                    SumEarnDeposit = g.Sum(x => x.EarnDeposit),//收取押金
                    SumPayDeposit = g.Sum(x => x.PayDeposit),//退还押金
                    SumEarnMonthEndPrice = g.Sum(x => x.EarnMonthEndPrice),//收入月底结算
                    VisitYear = g.First().VisitDate.Year + "年",
                    VisitMonth = g.First().VisitDate.Month + "月"//交易月份
                }).ToList();

            SumDailyRecordViewModel viewModel = new SumDailyRecordViewModel
            {
                SumDailyRecord = sumRecords,
                SumDailyRecordByCP = sumRecordsByCP
            };
            ViewBag.queryPam = JsonConvert.SerializeObject(new { YearMonth = yearMonth, StaffId = staffId });
            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.flag = "MonthEnd";
            return View(viewModel);
        }

        /// <summary>
        /// 查询月底日常汇总
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="StaffId"></param>
        /// <returns></returns>
        public ActionResult QueryMonthEnd(string yearMonth, string staffId, int pageSize = 1)
        {
            return RedirectToAction("MonthEnd", new { yearMonth = yearMonth, staffId = staffId, pageSize = pageSize });
        }
    }
}