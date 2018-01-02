using Common;
using Common.BusinessHelper;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers
{
    public class Home_CompanyController : BaseController
    {
        public ActionResult CompanyPay()
        {
            var companyPayType = CompanyPayTypeHelper.PayTypeList();
            var staffInfo = StaffHelper.StaffList();
            ViewBag.Staffs = staffInfo;

            ViewBag.flag = "CompanyPay";
            return View(companyPayType);
        }

        public ActionResult CompanyCreate(CompanyPayRecord companyPayRecord)
        {
            companyPayRecord.Id = ObjectId.NewObjectId().ToString();

            MongoBase.Insert<CompanyPayRecord>(companyPayRecord);
            return RedirectToAction("CompanyRecord");
        }

        public ActionResult CompanyRecord()
        {
            var companyPayRecords = TempData["queryRecords"] == null
                ? CompanyRecordHelper.CompanyList()
                : TempData["queryRecords"] as List<CompanyPayRecord>;

            var staffs = StaffHelper.StaffList();
            var paytypes = CompanyPayTypeHelper.PayTypeList();

            var payRecordInfo = (from r in companyPayRecords
                                 join s in staffs
                                 on r.StaffId equals s.Id
                                 join p in paytypes
                                 on r.PayTypeId equals p.Id
                                 select new CompanyPayRecordDesc()
                                 {
                                     StaffName = s.StaffName,
                                     PayTypeDesc = p.PayType,
                                     PaySum = r.PaySum,
                                     PayTime = r.PayTime,
                                     Describe = r.Describe
                                 }).ToList();
            ViewBag.Staffs = staffs;
            ViewBag.Paytypes = paytypes;

            ViewBag.flag = "CompanyRecord";
            return View(payRecordInfo);
        }

        public ActionResult QueryCompanyRecord(CompanyQueryMod queryMod)
        {
            var companyRecords = CompanyRecordHelper.CompanyList().Where(item => item.Id != "");
            if (!string.IsNullOrEmpty(queryMod.StaffId))
            {
                companyRecords = companyRecords.Where(item => item.StaffId == queryMod.StaffId);
            }
            if (!string.IsNullOrEmpty(queryMod.PayTypeId))
            {
                companyRecords = companyRecords.Where(item => item.PayTypeId == queryMod.PayTypeId);
            }
            if (queryMod.PayTimeBegin >= Convert.ToDateTime("2016-01-01"))
            {
                companyRecords = companyRecords.Where(item => item.PayTime >= queryMod.PayTimeBegin);
            }
            if (queryMod.PayTimeEnd >= Convert.ToDateTime("2016-01-01"))
            {
                companyRecords = companyRecords.Where(item => item.PayTime <= queryMod.PayTimeEnd);
            }

            TempData["queryRecords"] = companyRecords.ToList();

            return RedirectToAction("CompanyRecord");
        }
        

        public ActionResult CompanyEnd()
        {
            ViewBag.flag = "CompanyEnd";
            return View();
        }
    }
}