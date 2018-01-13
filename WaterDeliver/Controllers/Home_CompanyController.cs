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
            var salaryPay = companyPayType.FirstOrDefault(i => i.Id == SalaryPayType);
            //某些支出类型需关联水厂(押金支出与退回、进水..)
            var factories = FactoryHelper.FactoryList();
            factories.Insert(0, new Factory { Id = "", FactoryName = "" });
            if (salaryPay != null)
            {
                companyPayType.Remove(salaryPay);//公司交易中隐藏工资发放，工资发放在收支汇总中单独完成
            }
            var staffInfo = StaffHelper.StaffList();
            ViewBag.Staffs = staffInfo;
            ViewBag.Factories = factories;

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
            //获取页条数
            int pageSize = PageSize();
            var companyRecords = CompanyRecordHelper.CompanyList();
            var companyPayRecords = TempData["queryRecords"] == null
                ? companyRecords.OrderByDescending(item => item.TransTime).Skip(0).Take(pageSize)
                : TempData["queryRecords"] as List<CompanyPayRecord>;

            var staffs = StaffHelper.StaffList();
            var paytypes = CompanyPayTypeHelper.PayTypeList();

            var payRecordInfo = (from r in companyPayRecords
                                 join p in paytypes
                                 on r.PayTypeId equals p.Id
                                 join s in staffs
                                 on r.StaffId equals s.Id
                                 select new CompanyPayRecordDesc()
                                 {
                                     StaffName = s.StaffName,
                                     PayTypeDesc = p.PayType,
                                     IsPayType = r.IsPayType,
                                     TransSum = r.TransSum,
                                     TransTime = r.TransTime,
                                     Describe = r.Describe
                                 }).ToList();
            ViewBag.Staffs = staffs;
            ViewBag.Paytypes = paytypes;

            ViewBag.queryPam = TempData["queryMod"] == null ? "{}" : JsonConvert.SerializeObject(TempData["queryMod"]);

            ViewBag.totalPage = TempData["totalPage"] == null
                ? (companyRecords.Count() % pageSize == 0
                    ? companyRecords.Count() / pageSize
                    : Math.Ceiling(Convert.ToDouble(companyRecords.Count()) / pageSize))
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
            var companyRecords = CompanyRecordHelper.CompanyList().OrderByDescending(item => item.TransTime).Where(item => item.Id != "");
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
                companyRecords = companyRecords.Where(item => item.TransTime >= queryMod.PayTimeBegin);
            }
            if (queryMod.PayTimeEnd >= Convert.ToDateTime("2017-01-01"))
            {
                companyRecords = companyRecords.Where(item => item.TransTime <= queryMod.PayTimeEnd);
            }
            //获取页条数
            int pageSize = PageSize();
            var sumRecords = companyRecords.ToList();
            TempData["queryRecords"] = sumRecords
                .OrderByDescending(item => item.TransTime)
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
            //日常记录信息，用于统计提成
            var dailyRecords = DailyRecordHelper.DailyRecordList();

            //按年月分组
            var comRecordsMonth = companyRecords
                .OrderByDescending(item => item.TransTime)
                .GroupBy(item => new { item.TransTime.Year, item.TransTime.Month })
                .Select(g => new CompanyPayRecord
                {
                    TransTime = g.First().TransTime,
                    TransSum = g.Sum(i => i.IsPayType ? -i.TransSum : i.TransSum)
                }).ToList();

            //按年月、消费类型分组
            var comRecordsMonType = companyRecords
                .OrderByDescending(item => item.TransTime)
                .GroupBy(item => new { item.TransTime.Year, item.TransTime.Month, item.PayTypeId })
                .Select(g => new CompanyPayRecord
                {
                    TransTime = g.First().TransTime,
                    TransSum = g.Sum(i => i.IsPayType ? -i.TransSum : i.TransSum),
                    PayTypeId = g.First().PayTypeId
                }).Join(payType, x => x.PayTypeId, y => y.Id, (x, y) => new { x, y })
                .Select(p => new CompanyPayRecordDesc
                {
                    TransTime = p.x.TransTime,
                    TransSum = p.x.IsPayType ? -p.x.TransSum : p.x.TransSum,
                    PayTypeDesc = p.y.PayType
                }).ToList();

            //公司月底结算，动态加入员工提成
            double commission = Commission();
            foreach (var item in comRecordsMonth)
            {
                int sumBucket =
                    dailyRecords.Where(i => i.VisitDate.Year == item.TransTime.Year && i.VisitDate.Month == item.TransTime.Month)
                                .Sum(i => i.SendBucketAmount);
                //当月总支出减去员工提成
                item.TransSum -= sumBucket * commission;

                //年月消费类型集合中添加该月员工统计提成条目
                int index =
                    comRecordsMonType.FindLastIndex(t => t.TransTime.Year == item.TransTime.Year && t.TransTime.Month == item.TransTime.Month);
                comRecordsMonType.Insert(index + 1, new CompanyPayRecordDesc
                {
                    PayTypeDesc = "员工提成(" + commission + "元/桶)",
                    TransTime = item.TransTime,
                    TransSum = -sumBucket * commission
                });
            }

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
                companyRecords = companyRecords.Where(item => item.TransTime.Year == year && item.TransTime.Month == month).ToList();
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