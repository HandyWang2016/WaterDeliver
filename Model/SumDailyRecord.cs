using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// 日常记录汇总类
    /// </summary>
    public class SumDailyRecord
    {
        /// <summary>
        /// 送水桶数
        /// </summary>
        public int SumSendBucketAmount { get; set; }
        /// <summary>
        /// 收回空桶数
        /// </summary>
        public int SumReceiveEmptyBucketAmount { get; set; }
        /// <summary>
        /// 收取押金
        /// </summary>
        public double SumEarnDeposit { get; set; }
        /// <summary>
        /// 退还押金
        /// </summary>
        public double SumPayDeposit { get; set; }
        /// <summary>
        /// 收入月底结算
        /// </summary>
        public double SumEarnMonthEndPrice { get; set; }
        /// <summary>
        /// 收入水卡金额
        /// </summary>
        public double SumEarnWaterCardPrice { get; set; }
        /// <summary>
        /// 交易月份
        /// </summary>
        public string VisitMonth { get; set; }
    }

    public class SumDailyRecordByCP : SumDailyRecord
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
    }

    public class SumDailyRecordViewModel
    {
        public List<SumDailyRecord> SumDailyRecord { get; set; }
        public List<SumDailyRecordByCP> SumDailyRecordByCP { get; set; }
    }
}