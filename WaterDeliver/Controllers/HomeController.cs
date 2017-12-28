using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;
using System.Collections;

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
            //所有员工-客户关系信息
            var staffCustomers = StaffCustomerHelper.StaffCustomerList();
            //当前员工下的客户信息
            List<StaffCustomer> currentCustomers = staffCustomers;

            string staffId = GetStaffId();
            if (staffId == "") { RedirectToAction("index"); }
            currentCustomers = staffCustomers.Where(item => item.StaffId == staffId).ToList();
            //获取当前员工下的客户
            customers = (from c in customers
                         join cc in currentCustomers
                         on c.Id equals cc.CustomerId
                         select c).ToList();
            CustomerProductViewModel cusProduct = new CustomerProductViewModel()
            {
                Customers = customers,
                Products = products
            };

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
            dailyRecord.VisitDate = DateTime.Now;

            dailyRecord.StaffId = GetStaffId();
            if (dailyRecord.StaffId == "") { return RedirectToAction("index"); }

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
            //所有员工-客户关系信息
            var staffCustomers = StaffCustomerHelper.StaffCustomerList();
            //当前员工下的客户信息
            List<StaffCustomer> currentCustomers = staffCustomers;
            //所有产品
            var products = ProductHelper.ProductList();

            string staffId = GetStaffId();
            if (staffId == "") { return RedirectToAction("index"); }
            currentCustomers = staffCustomers.Where(item => item.StaffId == staffId).ToList();
            //获取当前员工下的客户
            customers = (from c in customers
                         join cc in currentCustomers
                         on c.Id equals cc.CustomerId
                         select c).ToList();
            //添加空选项
            customers.Insert(0, new Customer {CustomerName = ""});

            List<DailyRecord> records = TempData["DailyRecord"] == null
                ? DailyRecordHelper.DailyRecordList().Where(item => item.StaffId == staffId).ToList()
                : TempData["DailyRecord"] as List<DailyRecord>;
            List<DailyRecordShow> newRecords = (from r in records
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
                                                    EarnWaterCardPrice = r.EarnWaterCardPrice,
                                                    VisitDate = r.VisitDate

                                                }).ToList();

            ViewBag.flag = "DailyRecord";
            ViewBag.customers = customers;
            ViewBag.queryPam = TempData["dailyQuery"];
            return View(newRecords);
        }

        /// <summary>
        /// 查询日常记录
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <returns></returns>
        public ActionResult QueryDailyRecord(DailyQueryMod dailyQuery)
        {
            string staffId = GetStaffId();
            if (staffId == "") { return RedirectToAction("index"); }
            dailyQuery.StaffId = staffId;
            var records = FilterRecordInfo(dailyQuery);
            TempData["DailyRecord"] = records;
            TempData["dailyQuery"] = dailyQuery;
            return RedirectToAction("DailyRecord");
        }

        /// <summary>
        /// 根据查询条件过滤数据
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <returns></returns>
        private List<DailyRecord> FilterRecordInfo(DailyQueryMod dailyQuery)
        {
            List<DailyRecord> records = DailyRecordHelper.DailyRecordList();
            IEnumerable<DailyRecord> temRecords = records.Where(item => item.StaffId == dailyQuery.StaffId);
            if (!string.IsNullOrEmpty(dailyQuery.CustomerId))
            {
                temRecords = temRecords.Where(item => item.CustomerId == dailyQuery.CustomerId);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2016-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate >= dailyQuery.DateBegin);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2016-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate <= dailyQuery.DateEnd);
            }
            return temRecords.ToList<DailyRecord>();
        }

        public ActionResult MonthEnd()
        {
            ViewBag.flag = "MonthEnd";
            return View();
        }

        public ActionResult CompanyPay()
        {
            ViewBag.flag = "CompanyPay";
            return View();
        }

        public ActionResult CompanyRecord()
        {
            ViewBag.flag = "CompanyRecord";
            return View();
        }

        public ActionResult CompanyEnd()
        {
            ViewBag.flag = "CompanyEnd";
            return View();
        }
    }
}