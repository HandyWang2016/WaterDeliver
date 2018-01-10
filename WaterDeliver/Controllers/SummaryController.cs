using Common.BusinessHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Model;
using Newtonsoft.Json;

namespace WaterDeliver.Controllers
{
    public class SummaryController : BaseController
    {
        // GET: Summary
        public ActionResult Index(string yearMonth)
        {
            int year;
            int month;

            if (string.IsNullOrEmpty(yearMonth))
            {
                //默认显示上一个月的月底结算情况
                year = DateTime.Now.AddMonths(-1).Year;
                month = DateTime.Now.AddMonths(-1).Month;
                yearMonth = year + "-" + month;
            }
            else
            {
                year = int.Parse(yearMonth.Split('-')[0]);
                month = int.Parse(yearMonth.Split('-')[1]);
            }

            var dailyRecords =
                DailyRecordHelper.DailyRecordList()
                    .Where(item => item.VisitDate.Year == year && item.VisitDate.Month == month);
            var companyRecords =
                CompanyRecordHelper.CompanyList()
                    .Where(item => item.TransTime.Year == year && item.TransTime.Month == month);
            MonthEndSummary monthEnd = new MonthEndSummary();
            //员工汇总
            MonthEndSummary monthEnd1 = dailyRecords.GroupBy(item => new { item.VisitDate.Year, item.VisitDate.Month })
                .Select(g => new MonthEndSummary()
                {
                    StaffEarn =
                        g.Sum(x => x.EarnMonthEndPrice) + g.Sum(x => x.EarnWaterCardPrice) + g.Sum(x => x.EarnDeposit),
                    StaffPay = g.Sum(x => x.PayDeposit)

                }).FirstOrDefault();

            ////公司汇总
            //公司支出动态加入员工提成
            double commission = Commission();
            double sumBucketCom = dailyRecords.Sum(i => i.SendBucketAmount) * commission;

            MonthEndSummary monthEnd2 = companyRecords.GroupBy(item => new { item.TransTime.Year, item.TransTime.Month })
                .Select(g => new MonthEndSummary()
                {
                    CompanyEarn = g.Where(item => item.IsPayType == false).Sum(x => x.TransSum),
                    CompanyPay = g.Where(item => item.IsPayType && item.PayTypeId!= BucketPayType).Sum(x => x.TransSum) + sumBucketCom //进水支出不计入公司盈利运算
                }).FirstOrDefault();

            monthEnd.StaffEarn = monthEnd1?.StaffEarn ?? 0;
            monthEnd.StaffPay = monthEnd1?.StaffPay ?? 0;
            monthEnd.CompanyEarn = monthEnd2?.CompanyEarn ?? 0;
            monthEnd.CompanyPay = monthEnd2?.CompanyPay ?? 0;
            monthEnd.MonthEndEarn = monthEnd.StaffEarn + monthEnd.CompanyEarn - monthEnd.StaffPay - monthEnd.CompanyPay;
            monthEnd.YearMonth = year + "-" + month;

            ViewBag.queryPam = JsonConvert.SerializeObject(new { YearMonth = yearMonth });
            ViewBag.flag = "EndSummary";
            ViewBag.staffs = StaffHelper.StaffList();
            return View(monthEnd);
        }

        /// <summary>
        /// 工资发放
        /// </summary>
        /// <param name="staffSalary"></param>
        /// <returns></returns>
        public ActionResult PaySalary(StaffSalary staffSalary)
        {
            //检查该月份该员工是否已经发过工资
            var staffSal = SalaryHelper.CheckIfPaid(staffSalary.StaffId, staffSalary.SalaryMonth);
            if (staffSal != null)
            {
                TempData["CheckErr"] = "该员工" + staffSalary.SalaryMonth.Year + "-" + staffSalary.SalaryMonth.Month + "的薪资已经发放,请核查";
                return RedirectToAction("SalaryList");
            }

            //获取该员工该薪资月的送水记录，统计提成用
            var dailyRecords = DailyRecordHelper.DailyRecordList()
                .Where(item => item.VisitDate.Year == staffSalary.SalaryMonth.Year
                               && item.VisitDate.Month == staffSalary.SalaryMonth.Month
                               && item.StaffId == staffSalary.StaffId);
            //判断是否需要薪资发放类型
            var salaryType = CompanyPayTypeHelper.GetById(SalaryPayType);
            if (salaryType == null)
            {
                MongoBase.Insert(new CompanyPayType
                {
                    Id = SalaryPayType,
                    PayType = "员工薪资支出"
                });
            }

            staffSalary.Id = ObjectId.NewObjectId().ToString();
            staffSalary.Commission = dailyRecords.Sum(i => i.SendBucketAmount) * Commission(); //提成

            MongoBase.Insert(staffSalary);
            //公司当月支出增多相应金额
            CompanyPayRecord companyRecord = new CompanyPayRecord()
            {
                Id = ObjectId.NewObjectId().ToString(),
                IsPayType = true,
                PayTypeId = SalaryPayType,
                StaffId = staffSalary.StaffId,
                TransSum = staffSalary.Salary,
                TransTime = Convert.ToDateTime(staffSalary.SalaryMonth + "-28")
            };
            MongoBase.Insert(companyRecord);
            return RedirectToAction("SalaryList");
        }

        /// <summary>
        /// 员工工资查看列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryList(string staffId, string yearMonth)
        {
            var staffSalary = TempData["salaryRecords"] == null
                ? SalaryHelper.SalaryList()
                : TempData["salaryRecords"] as List<StaffSalary>;
            //读取员工
            var staff = StaffHelper.StaffList();
            var staffSalaryDesc =
                staffSalary.Join(staff, x => x.StaffId, y => y.Id, (x, y) => new { x, y }).Select(p => new StaffSalaryDesc
                {
                    StaffName = p.y.StaffName,
                    Salary = p.x.Salary,
                    Commission = p.x.Commission,
                    SalaryMonth = p.x.SalaryMonth,
                    MonthIncome = p.x.Commission + p.x.Salary
                });

            staff.Insert(0, new Staff { Id = "", StaffName = "" });
            ViewBag.staffs = staff;
            ViewBag.flag = "SalaryRecord";
            ViewBag.ErrInfo = TempData["CheckErr"] ?? "";
            ViewBag.queryPam = JsonConvert.SerializeObject(new { StaffId = staffId, YearMonth = yearMonth });
            return View(staffSalaryDesc);
        }

        public ActionResult QueryStaffSalary(string staffId, string yearMonth)
        {
            var staffSalary = SalaryHelper.SalaryList();
            if (!string.IsNullOrEmpty(yearMonth))
            {
                int year = int.Parse(yearMonth.Split('-')[0]);
                int month = int.Parse(yearMonth.Split('-')[1]);
                staffSalary =
                    staffSalary.Where(item => item.SalaryMonth.Year == year && item.SalaryMonth.Month == month).ToList();
            }
            if (!string.IsNullOrEmpty(staffId))
            {
                staffSalary = staffSalary.Where(item => item.StaffId == staffId).ToList();
            }

            TempData["salaryRecords"] = staffSalary;
            return RedirectToAction("SalaryList", new { staffId = staffId, yearMonth = yearMonth });

        }
    }
}