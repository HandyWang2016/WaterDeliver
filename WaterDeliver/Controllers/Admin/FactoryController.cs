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
            return View();
        }
    }
}