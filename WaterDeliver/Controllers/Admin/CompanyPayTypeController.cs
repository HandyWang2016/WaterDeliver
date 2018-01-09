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
    public class CompanyPayTypeController : Admin_BaseController
    {
        // GET: CompanyPayType
        public ActionResult Index()
        {
            var payTypes = CompanyPayTypeHelper.PayTypeList();
            var salaryPayType = payTypes.FirstOrDefault(i => i.Id == "50c8d301097facb82b660000");
            if (salaryPayType == null)
            {
                //初次加载，初始化薪资支出与进水支出类别，且不允许删除
                List<CompanyPayType> payTypeMods = new List<CompanyPayType>
                {
                    new CompanyPayType
                    {
                        Id = "50c8d301097facb82b660000",
                        PayType = "员工薪资支出"
                    },
                    new CompanyPayType
                    {
                        Id = "50c8d301097facb82b670000",
                        PayType = "进水支出"
                    }
                };
                MongoBase.Insert(payTypeMods);
            }
            ViewBag.flag = "payTypes";
            return View(payTypes);
        }

        public ActionResult Create(CompanyPayType companyPayType)
        {
            companyPayType.Id = ObjectId.NewObjectId().ToString();

            MongoBase.Insert<CompanyPayType>(companyPayType);
            return RedirectToAction("index");
        }

        public ActionResult Delete(string id)
        {
            CompanyPayTypeHelper.Delete(id);
            return RedirectToAction("Index");
        }
    }
}