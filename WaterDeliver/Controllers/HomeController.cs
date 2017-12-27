using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;

namespace WaterDeliver.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyWrite()
        {
            ViewBag.flag = "DailyWrite";
            return View();
        }

        public ActionResult DailyRecord()
        {
            ViewBag.flag = "DailyRecord";
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