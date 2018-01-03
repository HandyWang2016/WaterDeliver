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
    public class StaffCustomerController : Admin_BaseController
    {
        // GET: StaffProject
        public ActionResult Index(int pageIndex = 1)
        {
            var staffs = StaffHelper.StaffList();
            var customers = CustomerHelper.CustomerList();
            var staffCustomer = StaffCustomerHelper.StaffCustomerList();
            var currentStaffCustomers = staffCustomer.Skip((pageIndex - 1) * PageSize()).Take(PageSize()).ToList();

            StaffCustomerViewModel viewModel = new StaffCustomerViewModel()
            {
                Staffs = staffs,
                Customers = customers,
                StaffCustomers = currentStaffCustomers
            };

            ViewBag.totalPage = staffCustomer.Count() % PageSize() == 0
                    ? staffCustomer.Count() / PageSize()
                    : Math.Ceiling(Convert.ToDouble(staffCustomer.Count()) / PageSize());
            ViewBag.totalSize = staffCustomer.Count;
            ViewBag.currentPage = pageIndex;

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
            staffCustomer.Id = ObjectId.NewObjectId().ToString();
            MongoBase.Insert(staffCustomer);
            return RedirectToAction("Index");
        }
    }
}