﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;
using Newtonsoft.Json;

namespace WaterDeliver.Controllers.Admin
{
    public class ProductController : Admin_BaseController
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
            product.UpdateTime = DateTime.Now;

            MongoBase.Insert<Products>(product);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            ProductHelper.Delete(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 产品库存维护
        /// </summary>
        /// <returns></returns>
        public ActionResult StockMaintain()
        {
            ViewBag.flag = "stockMaintain";
            var products = ProductHelper.ProductList();
            if (TempData["TheProduct"] != null)
            {
                ViewBag.TheProduct = TempData["TheProduct"];
            }
            return View(products);
        }

        public ActionResult AddStockAmount(string productId, int numToAdd)
        {
            var product = ProductHelper.GetById(productId);
            product.StockRemain = product.StockRemain + numToAdd;
            product.UpdateTime = DateTime.Now;
            ProductHelper.Update(product);
            TempData["TheProduct"] = JsonConvert.SerializeObject(product);
            return RedirectToAction("StockMaintain");
        }
    }
}