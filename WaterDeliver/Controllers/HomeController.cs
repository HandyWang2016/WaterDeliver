using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;

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
            string staffId = GetStaffId();
            if (staffId == "") { return RedirectToAction("index"); }
            List<DailyRecord> records = DailyRecordHelper.StaffList();
            ViewBag.flag = "DailyRecord";
            return View(records);
        }

        public ActionResult QueryDailyRecord()
        {
            return View();
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