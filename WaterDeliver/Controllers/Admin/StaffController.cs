using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;

namespace WaterDeliver.Controllers.Admin
{
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            var staffs = StaffHelper.StaffList();
            ViewBag.flag = "staff";
            return View(staffs);
        }
        
        public ActionResult Create(Staff staff)
        {
            staff.Id = ObjectId.NewObjectId().ToString();
            MongoBase.Insert<Staff>(staff);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            StaffHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}