using Common.BusinessHelper;
using Model;
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
            var customers = CustomerHelper.CustomerList();

            var recordsAccessory = records.GroupBy(item => item.CustomerId)
                .Select(t => new CustomerAccessory
                {
                    CustomerId = t.First().CustomerId,
                    WaterDispenser = t.Sum(i => i.WaterDispenser),
                    WaterHolder = t.Sum(i => i.WaterHolder),
                    PushPump = t.Sum(i => i.PushPump)
                }).Join(customers, x => x.CustomerId, y => y.Id, (x, y) => new { x, y }).Select(p => new CustomerAccessory
                {
                    CustomerName = p.y.CustomerName,
                    WaterDispenser = p.x.WaterDispenser,
                    WaterHolder = p.x.WaterHolder,
                    PushPump = p.x.PushPump
                }).ToList();
            ViewBag.flag = "CustomerAccessory";
            return View(recordsAccessory);
        }
    }

    
}