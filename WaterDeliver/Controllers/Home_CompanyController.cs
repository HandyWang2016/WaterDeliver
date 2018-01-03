using Common;
using Common.BusinessHelper;
using Model;
using Newtonsoft.Json;
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
            var companyRecords = CompanyRecordHelper.CompanyList();
            var companyPayRecords = TempData["queryRecords"] == null
                ? companyRecords.Skip(0).Take(10)
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

            ViewBag.queryPam = TempData["queryMod"] == null ? "{}" : JsonConvert.SerializeObject(TempData["queryMod"]);

            ViewBag.totalPage = TempData["totalPage"] == null
                ? (companyRecords.Count() % 10 == 0
                    ? companyRecords.Count() / 10
                    : Math.Ceiling(Convert.ToDouble(companyRecords.Count()) / 10))
                : int.Parse(TempData["totalPage"].ToString());

            ViewBag.totalSize = TempData["totalSize"] == null
                ? companyRecords.Count
                : int.Parse(TempData["totalSize"].ToString());
            ViewBag.currentPage = TempData["currentPage"] == null
                 ? 1
                 : int.Parse(TempData["currentPage"].ToString());

            ViewBag.flag = "CompanyRecord";
            return View(payRecordInfo);
        }

        public ActionResult QueryCompanyRecord(CompanyQueryMod queryMod, int pageIndex = 1)
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
            if (queryMod.PayTimeBegin >= Convert.ToDateTime("2017-01-01"))
            {
                companyRecords = companyRecords.Where(item => item.PayTime >= queryMod.PayTimeBegin);
            }
            if (queryMod.PayTimeEnd >= Convert.ToDateTime("2017-01-01"))
            {
                companyRecords = companyRecords.Where(item => item.PayTime <= queryMod.PayTimeEnd);
            }
            //获取页条数
            int pageSize = PageSize();
            var sumRecords = companyRecords.ToList();
            TempData["queryRecords"] = sumRecords
                .OrderByDescending(item => item.PayTime)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            TempData["queryMod"] = queryMod;
            //记录分页相关字段
            TempData["totalPage"] = sumRecords.Count() % pageSize == 0
                ? sumRecords.Count() / pageSize
                : Math.Ceiling(Convert.ToDouble(sumRecords.Count()) / pageSize);
            TempData["totalSize"] = sumRecords.Count();
            TempData["currentPage"] = pageIndex;

            return RedirectToAction("CompanyRecord");
        }

        /// <summary>
        /// 公司结算汇总
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyEnd(string payTypeId, string yearMonth)
        {
            var companyRecords = TempData["companyRecords"] != null
                ? TempData["companyRecords"] as List<CompanyPayRecord>
                : CompanyRecordHelper.CompanyList();

            var payType = CompanyPayTypeHelper.PayTypeList();

            //按年月分组
            var comRecordsMonth = companyRecords
                .OrderByDescending(item => item.PayTime)
                .GroupBy(item => new { item.PayTime.Year, item.PayTime.Month })
                .Select(g => new CompanyPayRecord
                {
                    PayTime = g.First().PayTime,
                    PaySum = g.Sum(i => i.PaySum)
                }).ToList();

            //按年月、消费类型分组
            var comRecordsMonType = companyRecords
                .OrderByDescending(item => item.PayTime)
                .GroupBy(item => new { item.PayTime.Year, item.PayTime.Month, item.PayTypeId })
                .Select(g => new CompanyPayRecord
                {
                    PayTime = g.First().PayTime,
                    PaySum = g.Sum(i => i.PaySum),
                    PayTypeId = g.First().PayTypeId
                }).Join(payType, x => x.PayTypeId, y => y.Id, (x, y) => new { x, y })
                .Select(p => new CompanyPayRecordDesc
                {
                    PayTime = p.x.PayTime,
                    PaySum = p.x.PaySum,
                    PayTypeDesc = p.y.PayType
                }).ToList();
            CompanyPayRecordViewModel viewModel = new CompanyPayRecordViewModel()
            {
                CompanyPayRecord = comRecordsMonth,
                CompanyPayRecordDesc = comRecordsMonType
            };
            //记录查询条件
            ViewBag.queryPam = JsonConvert.SerializeObject(new { YearMonth = yearMonth, PayTypeId = payTypeId });
            ViewBag.flag = "CompanyEnd";
            ViewBag.PayType = payType;
            return View(viewModel);
        }

        public ActionResult QueryMonthEnd(string payTypeId, string yearMonth)
        {
            var companyRecords = CompanyRecordHelper.CompanyList();
            if (!string.IsNullOrEmpty(yearMonth))
            {
                int year = int.Parse(yearMonth.Split('-')[0]);
                int month = int.Parse(yearMonth.Split('-')[1]);
                companyRecords = companyRecords.Where(item => item.PayTime.Year == year && item.PayTime.Month == month).ToList();
            }
            if (!string.IsNullOrEmpty(payTypeId))
            {
                companyRecords = companyRecords.Where(item => item.PayTypeId == payTypeId).ToList();
            }

            TempData["companyRecords"] = companyRecords;
            return RedirectToAction("CompanyEnd", new { payTypeId = payTypeId, yearMonth = yearMonth });
        }
    }
}