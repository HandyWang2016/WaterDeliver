using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.BusinessHelper;

namespace WaterDeliver.Controllers
{
    public class Home_FactoryController : BaseController
    {
        /// <summary>
        /// 1. 水厂库存明细，统计字段：交易年月 产品名称 库存 空桶库存
        /// 2. 水厂交易明细 支付总押金 退还总押金 进水总支出
        /// </summary>
        /// <returns></returns>
        public ActionResult TransRecord()
        {
            var factories = FactoryHelper.FactoryList();
            var products = ProductHelper.ProductList();
            var companyRecords = CompanyRecordHelper.CompanyList();

            ViewBag.flag = "FactoryTransRecord";
            return View();
        }
    }
}