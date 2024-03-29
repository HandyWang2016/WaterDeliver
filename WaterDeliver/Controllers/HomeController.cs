﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.BusinessHelper;
using Model;
using System.Collections;
using Newtonsoft.Json;

namespace WaterDeliver.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            var customers = CustomerHelper.CustomerList().Where(i => i.NotifyFlag == 1).OrderBy(item => item.NextDate);
            return View(customers);
        }

        /// <summary>
        /// 日常记账
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyWrite()
        {
            //所有产品
            List<Products> products = ProductHelper.ProductList();
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            CustomerProductViewModel cusProduct = new CustomerProductViewModel()
            {
                Customers = customers,
                Products = products
            };
            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.flag = "DailyWrite";
            return View(cusProduct);
        }

        /// <summary>
        /// 添加日常记账
        /// </summary>
        /// <param name="dailyRecord"></param>
        /// <param name="hidBuckets"></param>
        /// <returns></returns>
        public ActionResult CreateDailyWrite(DailyRecord dailyRecord, string[] hidBuckets)
        {
            //MongoDB采取的是UTC时间，而通常系统用的是Local时间（中国）
            dailyRecord.VisitDate = DateTime.SpecifyKind(dailyRecord.VisitDate, DateTimeKind.Utc);
            string[] bucketPams = hidBuckets[0].Split(',');
            if (bucketPams.Length == 1)
            {
                if (dailyRecord.EarnMonthEndPrice >= 0 || dailyRecord.EarnWaterCardPrice >= 0)
                {
                    //添加日常记录
                    dailyRecord.Id = ObjectId.NewObjectId().ToString();
                    MongoBase.Insert(dailyRecord);

                    //添加公司下次送水日期
                    Customer customer = CustomerHelper.GetById(dailyRecord.CustomerId);
                    if (customer != null)
                    {
                        customer.NextDate = dailyRecord.NextDate;
                        customer.NotifyFlag = 1;//开启通知
                        CustomerHelper.Update(customer);
                    }
                }
            }
            else
            {
                for (int i = 0; i < bucketPams.Length; i++)
                {
                    var t = i % 3;
                    if (t == 0)
                    {
                        dailyRecord.SendProductId = bucketPams[i];
                    }
                    else if (t == 1)
                    {
                        dailyRecord.SendBucketAmount = int.Parse(bucketPams[i]);
                    }
                    else
                    {
                        dailyRecord.ReceiveEmptyBucketAmount = int.Parse(bucketPams[i]);
                    }
                    if ((i + 1) % 3 == 0)
                    {
                        ////完成一个产品
                        //更新库存信息
                        var product = ProductHelper.GetById(dailyRecord.SendProductId);
                        product.StockRemain += dailyRecord.SendBucketAmount;//桶装水库存增加
                        product.BucketStockRemain += dailyRecord.ReceiveEmptyBucketAmount;//空桶库存增加
                        ProductHelper.Update(product);
                        //添加一条日常记录
                        dailyRecord.Id = ObjectId.NewObjectId().ToString();

                        //同一人在同一公司，资金交易只记一笔(避免汇总数据double);附属产品只记一笔
                        if (i > 2)
                        {
                            dailyRecord.EarnDeposit = 0;
                            dailyRecord.PayDeposit = 0;
                            dailyRecord.EarnMonthEndPrice = 0;
                            dailyRecord.EarnWaterCardPrice = 0;
                            dailyRecord.WaterDispenser = 0;
                            dailyRecord.WaterHolder = 0;
                            dailyRecord.PushPump = 0;
                            dailyRecord.WaterDispenserBack = 0;
                            dailyRecord.WaterHolderBack = 0;
                            dailyRecord.PushPumpBack = 0;
                        }
                        MongoBase.Insert(dailyRecord);
                    }
                }
            }

            return RedirectToAction("DailyRecord");
        }

        /// <summary>
        /// 日常记账记录查看
        /// </summary>
        /// <returns></returns>
        public ActionResult DailyRecord()
        {
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            //所有产品
            var products = ProductHelper.ProductList();
            //所有员工
            var staffs = StaffHelper.StaffList();
            //所有日常记录
            var dailyRecords = TempData["DailyRecord"] == null ? DailyRecordHelper.DailyRecordList() : (List<DailyRecord>)TempData["DailyRecord"];
            //添加空选项
            customers.Insert(0, new Customer { CustomerName = "" });
            //获取页条数
            int pageSize = PageSize();

            List<DailyRecord> currentRecords = TempData["currentRecords"] == null
                ? dailyRecords
                .OrderByDescending(item => item.VisitDate)
                .Skip(0)
                .Take(pageSize)
                .ToList()
                : TempData["currentRecords"] as List<DailyRecord>;
            List<DailyRecordShow> newRecords = (from r in currentRecords
                                                join c in customers
                                                on r.CustomerId equals c.Id
                                                join p in products
                                                on r.SendProductId equals p.Id
                                                join s in staffs
                                                on r.StaffId equals s.Id
                                                select new DailyRecordShow
                                                {
                                                    StaffName = s.StaffName,
                                                    CustomerName = c.CustomerName,
                                                    ProductName = p.ProductName,
                                                    SendBucketAmount = r.SendBucketAmount,
                                                    ReceiveEmptyBucketAmount = r.ReceiveEmptyBucketAmount,
                                                    VisitDate = r.VisitDate,
                                                    DailyCost = r.SendBucketAmount * p.CostPrice,
                                                    Description = r.Description
                                                }).Where(item => item.SendBucketAmount > 0 || item.ReceiveEmptyBucketAmount > 0)
                                                .ToList();

            List<DailyRecordShow> recordsAll = (from r in dailyRecords
                                                join c in customers
                                                on r.CustomerId equals c.Id
                                                join p in products
                                                    on r.SendProductId equals p.Id
                                                join s in staffs
                                                    on r.StaffId equals s.Id
                                                select new DailyRecordShow
                                                {
                                                    StaffName = s.StaffName,
                                                    CustomerName = c.CustomerName,
                                                    ProductName = p.ProductName,
                                                    SendBucketAmount = r.SendBucketAmount,
                                                    ReceiveEmptyBucketAmount = r.ReceiveEmptyBucketAmount,
                                                    VisitDate = r.VisitDate,
                                                    DailyCost = r.SendBucketAmount * p.CostPrice,
                                                    Description = r.Description
                                                }).Where(item => item.SendBucketAmount > 0 || item.ReceiveEmptyBucketAmount > 0)
                                                .OrderByDescending(item => item.VisitDate)
                                                .ToList();

            //与公司的资金交易
            List<DailyFundTrans> fundRecords = dailyRecords.Where(
                                     item =>
                                         item.PayDeposit > 0 || item.EarnDeposit > 0 || item.EarnMonthEndPrice > 0 ||
                                         item.EarnWaterCardPrice > 0)
                                 //.GroupBy(item => new { item.VisitDate, item.CustomerId, item.StaffId })
                                 .Select(item => new DailyFundTrans()
                                 {
                                     StaffId = item.StaffId,
                                     CustomerId = item.CustomerId,
                                     EarnDeposit = item.EarnDeposit,
                                     PayDeposit = item.PayDeposit,
                                     EarnMonthEndPrice = item.EarnMonthEndPrice,
                                     EarnWaterCardPrice = item.EarnWaterCardPrice,
                                     Description = item.Description,
                                     VisitDate = item.VisitDate
                                 }).ToList();

            var recordsFundTrans = (from r in fundRecords
                                    join c in customers
                              on r.CustomerId equals c.Id
                                    join s in staffs
                                    on r.StaffId equals s.Id
                                    select new DailyFundTrans
                                    {
                                        StaffName = s.StaffName,
                                        CustomerName = c.CustomerName,
                                        EarnDeposit = r.EarnDeposit,
                                        PayDeposit = r.PayDeposit,
                                        EarnMonthEndPrice = r.EarnMonthEndPrice,
                                        EarnWaterCardPrice = r.EarnWaterCardPrice,
                                        Description = r.Description,
                                        VisitDate = r.VisitDate
                                    }).ToList();

            //附属产品交易
            var accessoryProRecords = dailyRecords.Where(
                    item =>
                        item.WaterHolder != 0 || item.WaterDispenser != 0 || item.PushPump != 0
                        || item.WaterHolderBack != 0 || item.WaterDispenserBack != 0 || item.PushPumpBack != 0)
                .GroupBy(item => new { item.VisitDate, item.CustomerId, item.StaffId })
                .Select(item => new AccessoryProducts()
                {
                    StaffId = item.First().StaffId,
                    CustomerId = item.First().CustomerId,
                    WaterDispenser = MakeAccessoryInfo(item.First().WaterDispenser, item.First().WaterDispenserBack),
                    PushPump = MakeAccessoryInfo(item.First().PushPump, item.First().PushPumpBack),
                    WaterHolder = MakeAccessoryInfo(item.First().WaterHolder, item.First().WaterHolderBack),
                    VisitDate = item.First().VisitDate
                });

            var recordsAccessoryPro = (from r in accessoryProRecords
                                       join c in customers
                              on r.CustomerId equals c.Id
                                       join s in staffs
                                       on r.StaffId equals s.Id
                                       select new AccessoryProducts
                                       {
                                           StaffName = s.StaffName,
                                           CustomerName = c.CustomerName,
                                           WaterHolder = r.WaterHolder,
                                           WaterDispenser = r.WaterDispenser,
                                           PushPump = r.PushPump,
                                           VisitDate = r.VisitDate
                                       }).ToList();

            ViewBag.flag = "DailyRecord";
            ViewBag.customers = customers;
            ViewBag.queryPam = TempData["dailyQuery"] == null ? "{}" : JsonConvert.SerializeObject(TempData["dailyQuery"]);

            ViewBag.totalPage = recordsAll.Count() % pageSize == 0
                ? recordsAll.Count() / pageSize
                : Math.Ceiling(Convert.ToDouble(recordsAll.Count()) / pageSize);

            ViewBag.totalSize = recordsAll.Count;
            ViewBag.currentPage = TempData["currentPage"] == null
                 ? 1
                 : int.Parse(TempData["currentPage"].ToString());

            ViewBag.recordsFundTrans = recordsFundTrans;
            ViewBag.recordsAccessoryPro = recordsAccessoryPro;
            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.recordsAll = recordsAll;
            return View(newRecords);
        }

        /// <summary>
        /// 日常记录--附属产品交易记录
        /// </summary>
        /// <returns></returns>
        public ActionResult AccessRecord()
        {
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            //所有员工
            var staffs = StaffHelper.StaffList();
            //所有日常记录
            var dailyRecords = (TempData["DailyRecord"] == null
                    ? DailyRecordHelper.DailyRecordList()
                    : (List<DailyRecord>)TempData["DailyRecord"]).Where(
                    item =>
                        item.WaterHolder != 0 || item.WaterDispenser != 0 || item.PushPump != 0
                        || item.WaterHolderBack != 0 || item.WaterDispenserBack != 0 || item.PushPumpBack != 0)
                .GroupBy(item => new { item.VisitDate, item.CustomerId, item.StaffId })
                .Select(item => new AccessoryProducts()
                {
                    StaffId = item.First().StaffId,
                    CustomerId = item.First().CustomerId,
                    WaterDispenser = MakeAccessoryInfo(item.First().WaterDispenser, item.First().WaterDispenserBack),
                    PushPump = MakeAccessoryInfo(item.First().PushPump, item.First().PushPumpBack),
                    WaterHolder = MakeAccessoryInfo(item.First().WaterHolder, item.First().WaterHolderBack),
                    VisitDate = item.First().VisitDate
                });

            //添加空选项
            customers.Insert(0, new Customer { CustomerName = "" });
            //获取页条数
            int pageSize = PageSize();

            List<AccessoryProducts> currentRecords = TempData["currentRecords"] == null
                ? dailyRecords
                .OrderByDescending(item => item.VisitDate)
                .Skip(0)
                .Take(pageSize)
                .ToList()
                : (List<AccessoryProducts>)TempData["currentRecords"];

            //附属产品交易--所有

            var recordsAccessoryPro = (from r in dailyRecords
                                       join c in customers
                              on r.CustomerId equals c.Id
                                       join s in staffs
                                       on r.StaffId equals s.Id
                                       select new AccessoryProducts
                                       {
                                           StaffName = s.StaffName,
                                           CustomerName = c.CustomerName,
                                           WaterHolder = r.WaterHolder,
                                           WaterDispenser = r.WaterDispenser,
                                           PushPump = r.PushPump,
                                           VisitDate = r.VisitDate
                                       }).ToList();

            //附属产品交易--当前页

            var acceCurrent = (from r in currentRecords
                               join c in customers
                      on r.CustomerId equals c.Id
                               join s in staffs
                               on r.StaffId equals s.Id
                               select new AccessoryProducts
                               {
                                   StaffName = s.StaffName,
                                   CustomerName = c.CustomerName,
                                   WaterHolder = r.WaterHolder,
                                   WaterDispenser = r.WaterDispenser,
                                   PushPump = r.PushPump,
                                   VisitDate = r.VisitDate
                               }).ToList();

            ViewBag.flag = "DailyRecord";
            ViewBag.customers = customers;
            ViewBag.queryPam = TempData["dailyQuery"] == null ? "{}" : JsonConvert.SerializeObject(TempData["dailyQuery"]);

            ViewBag.totalPage = dailyRecords.Count() % pageSize == 0
                ? dailyRecords.Count() / pageSize
                : Math.Ceiling(Convert.ToDouble(dailyRecords.Count()) / pageSize);

            ViewBag.totalSize = dailyRecords.Count();
            ViewBag.currentPage = TempData["currentPage"] == null
                 ? 1
                 : int.Parse(TempData["currentPage"].ToString());

            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.recordsAll = recordsAccessoryPro;
            return View(acceCurrent);
        }

        /// <summary>
        /// 日常记录--公司资金交易
        /// </summary>
        /// <returns></returns>
        public ActionResult FundRecord()
        {
            //所有客户
            List<Customer> customers = CustomerHelper.CustomerList();
            //所有产品
            var products = ProductHelper.ProductList();
            //所有员工
            var staffs = StaffHelper.StaffList();
            //所有日常记录
            var dailyRecords = (TempData["DailyRecord"] == null ? DailyRecordHelper.DailyRecordList() : (List<DailyRecord>)TempData["DailyRecord"]).Where(
                item =>
                    item.PayDeposit > 0 || item.EarnDeposit > 0 || item.EarnMonthEndPrice > 0 ||
                    item.EarnWaterCardPrice > 0).ToList();
            //添加空选项
            customers.Insert(0, new Customer { CustomerName = "" });
            //获取页条数
            int pageSize = PageSize();

            List<DailyRecord> currentRecords = TempData["currentRecords"] == null
                ? dailyRecords
                .OrderByDescending(item => item.VisitDate)
                .Skip(0)
                .Take(pageSize)
                .ToList()
                : TempData["currentRecords"] as List<DailyRecord>;

            //与公司的资金交易--当前页
            List<DailyFundTrans> fundRecordsCurrent = currentRecords
                                 .Select(item => new DailyFundTrans()
                                 {
                                     StaffId = item.StaffId,
                                     CustomerId = item.CustomerId,
                                     EarnDeposit = item.EarnDeposit,
                                     PayDeposit = item.PayDeposit,
                                     EarnMonthEndPrice = item.EarnMonthEndPrice,
                                     EarnWaterCardPrice = item.EarnWaterCardPrice,
                                     Description = item.Description,
                                     VisitDate = item.VisitDate
                                 }).ToList();

            var recordsFundTransCurrent = (from r in fundRecordsCurrent
                                           join c in customers
                                     on r.CustomerId equals c.Id
                                           join s in staffs
                                           on r.StaffId equals s.Id
                                           select new DailyFundTrans
                                           {
                                               StaffName = s.StaffName,
                                               CustomerName = c.CustomerName,
                                               EarnDeposit = r.EarnDeposit,
                                               PayDeposit = r.PayDeposit,
                                               EarnMonthEndPrice = r.EarnMonthEndPrice,
                                               EarnWaterCardPrice = r.EarnWaterCardPrice,
                                               Description = r.Description,
                                               VisitDate = r.VisitDate
                                           }).ToList();

            //与公司的资金交易--所有
            List<DailyFundTrans> fundRecords = dailyRecords
                                 .Select(item => new DailyFundTrans()
                                 {
                                     StaffId = item.StaffId,
                                     CustomerId = item.CustomerId,
                                     EarnDeposit = item.EarnDeposit,
                                     PayDeposit = item.PayDeposit,
                                     EarnMonthEndPrice = item.EarnMonthEndPrice,
                                     EarnWaterCardPrice = item.EarnWaterCardPrice,
                                     Description = item.Description,
                                     VisitDate = item.VisitDate
                                 }).OrderByDescending(item => item.VisitDate).ToList();

            var recordsFundTrans = (from r in fundRecords
                                    join c in customers
                              on r.CustomerId equals c.Id
                                    join s in staffs
                                    on r.StaffId equals s.Id
                                    select new DailyFundTrans
                                    {
                                        StaffName = s.StaffName,
                                        CustomerName = c.CustomerName,
                                        EarnDeposit = r.EarnDeposit,
                                        PayDeposit = r.PayDeposit,
                                        EarnMonthEndPrice = r.EarnMonthEndPrice,
                                        EarnWaterCardPrice = r.EarnWaterCardPrice,
                                        Description = r.Description,
                                        VisitDate = r.VisitDate
                                    }).ToList();

            ViewBag.flag = "DailyRecord";
            ViewBag.customers = customers;
            ViewBag.queryPam = TempData["dailyQuery"] == null ? "{}" : JsonConvert.SerializeObject(TempData["dailyQuery"]);

            ViewBag.totalPage = dailyRecords.Count() % pageSize == 0
                ? dailyRecords.Count() / pageSize
                : Math.Ceiling(Convert.ToDouble(dailyRecords.Count()) / pageSize);

            ViewBag.totalSize = dailyRecords.Count;
            ViewBag.currentPage = TempData["currentPage"] == null
                 ? 1
                 : int.Parse(TempData["currentPage"].ToString());

            ViewBag.recordsFundTrans = recordsFundTrans;
            ViewBag.Staffs = StaffHelper.StaffList();
            return View(recordsFundTransCurrent);
        }

        /// <summary>
        /// 拼接附属产品送出、回购数量的字符串 
        /// 格式eg：0  0/1  1/1  1/0
        /// </summary>
        /// <param name="sendNum"></param>
        /// <param name="receiveNum"></param>
        /// <returns></returns>
        private string MakeAccessoryInfo(int sendNum, int receiveNum)
        {
            return sendNum + receiveNum == 0 ? "0" : (sendNum + "/" + receiveNum);
        }

        /// <summary>
        /// 查询日常记录
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public ActionResult QueryDailyRecord(DailyQueryMod dailyQuery, int pageIndex, int flag)
        {
            int pageSize = PageSize();
            //获取当页信息
            var records = FilterRecordInfo(dailyQuery, pageIndex);
            TempData["currentRecords"] = records;
            TempData["dailyQuery"] = dailyQuery;
            switch (flag)
            {
                case 0:
                    return RedirectToAction("DailyRecord");
                case 1://附属产品交易
                    TempData["currentRecords"] = ((List<DailyRecord>)TempData["DailyRecord"]).Where(
                    item =>
                        item.WaterHolder != 0 || item.WaterDispenser != 0 || item.PushPump != 0
                        || item.WaterHolderBack != 0 || item.WaterDispenserBack != 0 || item.PushPumpBack != 0)
                        .GroupBy(item => new { item.VisitDate, item.CustomerId, item.StaffId })
                        .Select(item => new AccessoryProducts()
                        {
                            StaffId = item.First().StaffId,
                            CustomerId = item.First().CustomerId,
                            WaterDispenser = MakeAccessoryInfo(item.First().WaterDispenser, item.First().WaterDispenserBack),
                            PushPump = MakeAccessoryInfo(item.First().PushPump, item.First().PushPumpBack),
                            WaterHolder = MakeAccessoryInfo(item.First().WaterHolder, item.First().WaterHolderBack),
                            VisitDate = item.First().VisitDate
                        })
                        .OrderByDescending(item => item.VisitDate)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize).ToList();

                    return RedirectToAction("AccessRecord");
                case 2://公司资金交易
                    TempData["currentRecords"] = ((List<DailyRecord>)TempData["DailyRecord"]).Where(
                            item =>
                                item.PayDeposit > 0 || item.EarnDeposit > 0 || item.EarnMonthEndPrice > 0 ||
                                item.EarnWaterCardPrice > 0)
                        .OrderByDescending(item => item.VisitDate)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize).ToList();

                    return RedirectToAction("FundRecord");
                default:
                    return RedirectToAction("DailyRecord");
            }

        }

        /// <summary>
        /// 根据查询条件过滤数据
        /// </summary>
        /// <param name="dailyQuery"></param>
        /// <returns></returns>
        private List<DailyRecord> FilterRecordInfo(DailyQueryMod dailyQuery, int pageIndex)
        {
            List<DailyRecord> records = DailyRecordHelper.DailyRecordList();
            IEnumerable<DailyRecord> temRecords = records.Where(item => item.StaffId != "");
            if (!string.IsNullOrEmpty(dailyQuery.StaffId))
            {
                temRecords = temRecords.Where(item => item.StaffId == dailyQuery.StaffId);
            }
            if (!string.IsNullOrEmpty(dailyQuery.CustomerId))
            {
                temRecords = temRecords.Where(item => item.CustomerId == dailyQuery.CustomerId);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2017-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate >= dailyQuery.DateBegin);
            }
            if (dailyQuery.DateBegin > Convert.ToDateTime("2017-01-01"))
            {
                temRecords = temRecords.Where(item => item.VisitDate <= dailyQuery.DateEnd.AddHours(8));
            }
            temRecords = temRecords.Where(item => item.SendBucketAmount > 0 || item.ReceiveEmptyBucketAmount > 0);
            //获取页条数
            int pageSize = PageSize();
            var currentRecords = temRecords
                 .OrderByDescending(item => item.VisitDate)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize);
            TempData["currentPage"] = pageIndex;
            TempData["DailyRecord"] = temRecords.ToList();

            return currentRecords.ToList<DailyRecord>();
        }

        /// <summary>
        /// 月底结算
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthEnd(string yearMonth, string staffId, int pageSize = 1)
        {
            var records = DailyRecordHelper.DailyRecordList();
            var customers = CustomerHelper.CustomerList();
            var products = ProductHelper.ProductList();
            var staffs = StaffHelper.StaffList();
            if (!string.IsNullOrEmpty(yearMonth))
            {
                int year = int.Parse(yearMonth.Split('-')[0]);
                int month = int.Parse(yearMonth.Split('-')[1]);
                records = records.Where(item => item.VisitDate.Year == year && item.VisitDate.Month == month).ToList();
            }
            if (!string.IsNullOrEmpty(staffId))
            {
                records = records.Where(item => item.StaffId == staffId.ToString()).ToList();
            }

            //按月份、人员分组，计算员工的月送水成本
            List<MonthEndStaffwaterCost> staffCost = records
                .Join(products, x => x.SendProductId, y => y.Id, (x, y) => new { x, y })
                .GroupBy(item => new
                {
                    item.x.VisitDate.Year,
                    item.x.VisitDate.Month,
                    item.x.StaffId,
                    item.x.SendProductId,
                    item.x.SendBucketAmount,
                    item.y.CostPrice,
                    item.x.CustomerId
                })
                .Select(p => new MonthEndStaffwaterCost()
                {
                    VisitYear = p.First().x.VisitDate.Year.ToString(),
                    VisitMonth = p.First().x.VisitDate.Month.ToString(),
                    StaffId = p.First().x.StaffId,
                    WaterCost = p.First().x.SendBucketAmount * p.First().y.CostPrice
                }).GroupBy(item => new { item.VisitYear, item.VisitMonth, item.StaffId }).Select(p =>
                      new MonthEndStaffwaterCost()
                      {
                          VisitYear = p.First().VisitYear,
                          VisitMonth = p.First().VisitMonth,
                          StaffId = p.First().StaffId,
                          WaterCost = p.Sum(item => item.WaterCost)
                      }).Join(staffs, x => x.StaffId, y => y.Id, (x, y) => new { x, y }).Select(p =>
                      new MonthEndStaffwaterCost
                      {
                          VisitYear = p.x.VisitYear,
                          VisitMonth = p.x.VisitMonth,
                          StaffName = p.y.StaffName,
                          WaterCost = p.x.WaterCost
                      }).ToList();

            //按月份、公司分组
            List<SumDailyRecordByCP> sumRecordsByCP = records.OrderByDescending(i => i.VisitDate)
                .GroupBy(item => new { item.VisitDate.Year, item.VisitDate.Month, item.CustomerId })
                .Select(g => new SumDailyRecordByCP
                {
                    SumSendBucketAmount = g.Sum(x => x.SendBucketAmount), //送水桶数
                    SumReceiveEmptyBucketAmount = g.Sum(x => x.ReceiveEmptyBucketAmount), //收回空桶数
                    SumEarnDeposit = g.Sum(x => x.EarnDeposit), //收取押金
                    SumPayDeposit = g.Sum(x => x.PayDeposit), //退还押金
                    SumEarnMonthEndPrice = g.Sum(x => x.EarnMonthEndPrice), //收入月底结算
                    SumEarnWaterCardPrice = g.Sum(x => x.EarnWaterCardPrice), //收入水卡金额
                    VisitMonth = g.First().VisitDate.Month + "月", //交易月份
                    CustomerName = g.First().CustomerId,
                    ProductName = g.First().SendProductId
                }).Join(customers, x => x.CustomerName, y => y.Id, (x, y) => new { x, y }).Select(p => new SumDailyRecordByCP()
                {
                    CustomerName = p.y.CustomerName,
                    SumSendBucketAmount = p.x.SumSendBucketAmount,
                    SumReceiveEmptyBucketAmount = p.x.SumReceiveEmptyBucketAmount,
                    SumEarnDeposit = p.x.SumEarnDeposit,
                    SumPayDeposit = p.x.SumPayDeposit,
                    SumEarnMonthEndPrice = p.x.SumEarnMonthEndPrice,
                    SumEarnWaterCardPrice = p.x.SumEarnWaterCardPrice,
                    VisitMonth = p.x.VisitMonth
                }).ToList();


            //按月份分组
            List<SumDailyRecord> sumRecords = records.OrderByDescending(i => i.VisitDate)
                .GroupBy(item => new { item.VisitDate.Year, item.VisitDate.Month })
                .Select(g => new SumDailyRecord
                {
                    SumSendBucketAmount = g.Sum(x => x.SendBucketAmount),//送水桶数
                    SumReceiveEmptyBucketAmount = g.Sum(x => x.ReceiveEmptyBucketAmount),//收回空桶数
                    SumEarnDeposit = g.Sum(x => x.EarnDeposit),//收取押金
                    SumPayDeposit = g.Sum(x => x.PayDeposit),//退还押金
                    SumEarnMonthEndPrice = g.Sum(x => x.EarnMonthEndPrice),//收入月底结算
                    SumEarnWaterCardPrice = g.Sum(x => x.EarnWaterCardPrice),//收入水卡金额
                    VisitYear = g.First().VisitDate.Year + "年",
                    VisitMonth = g.First().VisitDate.Month + "月"//交易月份
                }).ToList();

            SumDailyRecordViewModel viewModel = new SumDailyRecordViewModel
            {
                SumDailyRecord = sumRecords,
                SumDailyRecordByCP = sumRecordsByCP,
                SumSendWatercost = staffCost
            };
            ViewBag.queryPam = JsonConvert.SerializeObject(new { YearMonth = yearMonth, StaffId = staffId });
            ViewBag.Staffs = StaffHelper.StaffList();
            ViewBag.flag = "MonthEnd";
            return View(viewModel);
        }

        /// <summary>
        /// 查询月底日常汇总
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="StaffId"></param>
        /// <returns></returns>
        public ActionResult QueryMonthEnd(string yearMonth, string staffId, int pageSize = 1)
        {
            return RedirectToAction("MonthEnd", new { yearMonth = yearMonth, staffId = staffId, pageSize = pageSize });
        }

        #region 以公司为主体，截至到现在的产品交易汇总及明细
        /// <summary>
        /// 截至到现在，公司的所有送水记录及汇总记录
        /// </summary>
        /// <returns></returns>
        public ActionResult CompanyRecordsUptonow()
        {
            var records = DailyRecordHelper.DailyRecordList();
            var customers = CustomerHelper.CustomerList();
            var companyDailyRecords = records.GroupBy(item => item.CustomerId).Select(p => new CustomerAccessory()
            {
                CustomerId = p.First().CustomerId,
                SendBuckets = p.Sum(i => i.SendBucketAmount),
                ReceiveEmptyBuckets = p.Sum(i => i.ReceiveEmptyBucketAmount),
                WaterDispenser = p.Sum(i => i.WaterDispenser),
                WaterHolder = p.Sum(i => i.WaterHolder),
                PushPump = p.Sum(i => i.PushPump)
            }).Join(customers, x => x.CustomerId, y => y.Id, (x, y) => new { x, y }).Select(p => new CustomerAccessory
            {
                CustomerId = p.x.CustomerId,
                CustomerName = p.y.CustomerName,
                SendBuckets = p.x.SendBuckets,
                ReceiveEmptyBuckets = p.x.ReceiveEmptyBuckets,
                WaterDispenser = p.x.WaterDispenser,
                WaterHolder = p.x.WaterHolder,
                PushPump = p.x.PushPump
            });

            ViewBag.flag = "CompanyRecordsUptonow";
            return View(companyDailyRecords);
        }

        public ActionResult CompanyDailyRecordsDetails(string customerId, string customerName, int pageIndex)
        {
            //该公司的所有日常交易记录
            var records = DailyRecordHelper.DailyRecordList()
                .Where(item => item.CustomerId == customerId)
                .OrderByDescending(item => item.VisitDate)
                .ToList();
            CustomerAccessory sumaryMod = records.GroupBy(item => item.CustomerId).Select(p => new CustomerAccessory()
            {
                SendBuckets = p.Sum(i => i.SendBucketAmount),
                ReceiveEmptyBuckets = p.Sum(i => i.ReceiveEmptyBucketAmount),
                WaterDispenser = p.Sum(i => i.WaterDispenser),
                WaterHolder = p.Sum(i => i.WaterHolder),
                PushPump = p.Sum(i => i.PushPump)
            }).First();

            //获取页条数
            int pageSize = PageSize();
            var currentRecords = records
                 .OrderByDescending(item => item.VisitDate)
                 .Skip((pageIndex - 1) * pageSize)
                 .Take(pageSize);
            //记录分页相关字段
            ViewBag.totalPage = records.Count() % pageSize == 0
               ? records.Count() / pageSize
               : Math.Ceiling(Convert.ToDouble(records.Count()) / pageSize);
            ViewBag.totalSize = records.Count();
            ViewBag.currentPage = pageIndex;

            ViewBag.customerName = customerName;
            ViewBag.customerId = customerId;
            ViewBag.sumaryInfo = sumaryMod;
            ViewBag.recordsAll = records;
            ViewBag.flag = "CompanyRecordsUptonow";
            return View(currentRecords);
        }

        #endregion
    }
}