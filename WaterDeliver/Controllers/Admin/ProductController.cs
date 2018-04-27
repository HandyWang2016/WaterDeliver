using System;
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
            if (TempData["isRepeat"] != null)
            {
                ViewBag.Err = "该产品已存在，请修改名称再试";
                TempData.Remove("isRepeat");
            }
            var factories = FactoryHelper.FactoryList();
            var products = ProductHelper.ProductList();

            var newProducts = products.OrderByDescending(i => i.UpdateTime).Join(factories, x => x.FactoryId, y => y.Id, (x, y) => new ProductFac
            {
                Id = x.Id,
                ProductName = x.ProductName,
                CostPrice = x.CostPrice,
                StockRemain = x.StockRemain,
                BucketStockRemain = x.BucketStockRemain,
                BucketCostPrice = x.BucketCostPrice,
                SalePrice = x.SalePrice,
                UpdateTime = x.UpdateTime,
                FactoryName = y.FactoryName
            });

            ViewBag.Factories = factories;
            ViewBag.flag = "product";
            return View(newProducts);
        }

        public ActionResult Create(Products product)
        {
            //重名判断
            var pro = ProductHelper.GetByName(product.ProductName);
            if (pro != null)
            {
                TempData["isRepeat"] = true;
            }
            else
            {
                product.Id = ObjectId.NewObjectId().ToString();
                product.UpdateTime = DateTime.Now;
                MongoBase.Insert(product);
            }
            
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            ProductHelper.Delete(id);
            DailyRecordHelper.DeleteMany(id);//日常记录中与该产品相关的记录都会删除
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

        public ActionResult AddStockAmount(string productId, int numToAdd, int numToReadd)
        {
            var product = ProductHelper.GetById(productId);
            product.StockRemain = product.StockRemain + numToAdd;
            product.BucketStockRemain = product.BucketStockRemain - numToReadd;
            product.UpdateTime = DateTime.Now;
            ProductHelper.Update(product);
            TempData["TheProduct"] = JsonConvert.SerializeObject(product);
            return RedirectToAction("StockMaintain");
        }
    }
}