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
    public class StaffCustomerController : Controller
    {
        // GET: StaffProject
        public ActionResult Index()
        {
            var staffs = StaffHelper.StaffList();
            var customers = CustomerHelper.CustomerList();
            var staffCustomer = StaffCustomerHelper.StaffCustomerList();

            StaffCustomerViewModel viewModel = new StaffCustomerViewModel()
            {
                Staffs = staffs,
                Customers = customers,
                StaffCustomers = staffCustomer
            };

            ViewBag.flag = "stacus";
            return View(viewModel);
        }

        public ActionResult Delete(string id)
        {
            StaffCustomerHelper.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult Create(StaffCustomer staffCustomer)
        {
            staffCustomer.Id= ObjectId.NewObjectId().ToString();
            MongoBase.Insert(staffCustomer);
            return RedirectToAction("Index");
        }
    }
}