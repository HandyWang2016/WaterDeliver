using Common.BusinessHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers
{
    /// <summary>
    /// 公司的附属产品使用情况
    /// </summary>
    public class Home_AccessoryController : BaseController
    {
        // GET: Home_Accessory
        public ActionResult Index()
        {
            var records = DailyRecordHelper.DailyRecordList();

            return View();
        }
    }
}