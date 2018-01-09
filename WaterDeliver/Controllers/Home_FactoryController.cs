using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.BusinessHelper;

namespace WaterDeliver.Controllers
{
    public class Home_FactoryController : BaseController
    {
        // GET: Factory
        public ActionResult TransRecord()
        {
            var factories = FactoryHelper.FactoryList();
            var products = ProductHelper.ProductList();
            ViewBag.flag = "FactoryTransRecord";
            return View();
        }
    }
}