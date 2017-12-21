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
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var products = ProductHelper.ProductList();
            ViewBag.flag = "product";
            return View(products);
        }

        public ActionResult Create(Products product)
        {
            product.Id = ObjectId.NewObjectId().ToString();
            MongoBase.Insert<Products>(product);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            ProductHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}