using Common;
using Common.BusinessHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers.Admin
{
    public class CustomerController : Admin_BaseController
    {
        // GET: Product
        public ActionResult Index(int pageIndex = 1)
        {
            var customers = CustomerHelper.CustomerList();
            var currentCustomers = customers.Skip((pageIndex - 1) * PageSize()).Take(PageSize()).ToList();

            ViewBag.totalPage = customers.Count() % PageSize() == 0
                    ? customers.Count() / PageSize()
                    : Math.Ceiling(Convert.ToDouble(customers.Count()) / PageSize());
            ViewBag.totalSize = customers.Count;
            ViewBag.currentPage = pageIndex;

            ViewBag.flag = "customer";
            return View(currentCustomers);
        }

        public ActionResult Create(Customer customer)
        {
            customer.Id = ObjectId.NewObjectId().ToString();

            MongoBase.Insert<Customer>(customer);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            CustomerHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}