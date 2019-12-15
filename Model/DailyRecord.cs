using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DailyRecord
    {
        public string Id { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public string StaffId { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 所送产品Id(可能为饮水机)
        /// </summary>
        public string SendProductId { get; set; }
        /// <summary>
        /// 送水桶数
        /// </summary>
        public int SendBucketAmount { get; set; }
        /// <summary>
        /// 收回空桶数
        /// </summary>
        public int ReceiveEmptyBucketAmount { get; set; }
        /// <summary>
        /// 收入押金金额
        /// </summary>
        public double EarnDeposit { get; set; }
        /// <summary>
        /// 支出押金金额
        /// </summary>
        public double PayDeposit { get; set; }
        /// <summary>
        /// 收入月底结算金额
        /// </summary>
        public double EarnMonthEndPrice { get; set; }
        /// <summary>
        /// 收入水卡金额
        /// </summary>
        public double EarnWaterCardPrice { get; set; }

        /// <summary>
        /// 描述(押金或月底结算可能入了个人账号，等等情况，在此说明)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 纪录日期
        /// </summary>
        public DateTime VisitDate { get; set; }
        /// <summary>
        /// 下次送水日期(客户表字段)
        /// </summary>
        public DateTime NextDate { get; set; }

        /// <summary>
        /// 附属产品：饮水机，水支架，手压泵
        /// </summary>
        public int WaterDispenser { get; set; }
        public int WaterHolder { get; set; }
        public int PushPump { get; set; }
        public int WaterDispenserBack { get; set; }
        public int WaterHolderBack { get; set; }
        public int PushPumpBack { get; set; }
    }

    public class DailyRecordShow : DailyRecord
    {
        public string StaffName { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        //每天送水成本
        public double DailyCost { get; set; }
    }

    public class DailyFundTrans
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double EarnDeposit { get; set; }
        public double PayDeposit { get; set; }
        public double EarnMonthEndPrice { get; set; }
        public double EarnWaterCardPrice { get; set; }
        public string Description { get; set; }
        public DateTime VisitDate { get; set; }
    }

    /// <summary>
    /// 附属产品列表：饮水机，水支架，手压泵
    /// </summary>
    public class AccessoryProducts
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string WaterDispenser { get; set; }
        public string WaterHolder { get; set; }
        public string PushPump { get; set; }
        public DateTime VisitDate { get; set; }
    }


}
