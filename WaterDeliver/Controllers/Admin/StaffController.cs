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
            MongoBase.Insert<Staff>(staff);
            return View("Index");
        }

        //public ActionResult Update(int id)
        //{
        //    Staff mod = StaffHelper.GetById(id);
        //}

        public ActionResult Delete(int id)
        {
            StaffHelper.Delete(id);
            return View("Index");
        }
    }
}