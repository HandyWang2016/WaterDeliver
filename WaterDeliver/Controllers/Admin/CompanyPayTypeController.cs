using Common;
using Common.BusinessHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers.Admin
{
    public class CompanyPayTypeController : Admin_BaseController
    {
        // GET: CompanyPayType
        public ActionResult Index()
        {
            var payTypes = CompanyPayTypeHelper.PayTypeList();
            ViewBag.flag = "payTypes";
            return View(payTypes);
        }

        public ActionResult Create(CompanyPayType companyPayType)
        {
            companyPayType.Id = ObjectId.NewObjectId().ToString();

            MongoBase.Insert<CompanyPayType>(companyPayType);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            CompanyPayTypeHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}