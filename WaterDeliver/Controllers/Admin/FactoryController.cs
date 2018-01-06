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
    /// <summary>
    /// 水厂管理
    /// </summary>
    public class FactoryController : Controller
    {
        // GET: Factory
        public ActionResult Index()
        {
            var factories = FactoryHelper.FactoryList();
            ViewBag.flag = "factory";
            return View(factories);
        }

        public ActionResult Create(Factory factory)
        {
            factory.Id = ObjectId.NewObjectId().ToString();
            MongoBase.Insert<Factory>(factory);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            StaffHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}