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
    public class CustomerController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var customers = CustomerHelper.CustomerList();
            ViewBag.flag = "customer";
            return View(customers);
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