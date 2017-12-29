using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers
{
    public class Home_CompanyController : BaseController
    {
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