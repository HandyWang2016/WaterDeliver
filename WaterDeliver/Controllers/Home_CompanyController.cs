using Common;
using Common.BusinessHelper;
using Model;
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
            var companyPayType = CompanyPayTypeHelper.PayTypeList();
            ViewBag.flag = "CompanyPay";
            return View(companyPayType);
        }

        public ActionResult CompanyCreate(CompanyPayRecord companyPayRecord)
        {
            companyPayRecord.Id = ObjectId.NewObjectId().ToString();
            companyPayRecord.PayTime = DateTime.Now;

            MongoBase.Insert<CompanyPayRecord>(companyPayRecord);
            return RedirectToAction("CompanyPay");
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