using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DailyRecord
    {
        public int Id { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public int StaffId { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 所送产品Id(可能为饮水机)
        /// </summary>
        public int SendProductId { get; set; }
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
        public decimal EarnDeposit { get; set; }
        /// <summary>
        /// 支出押金金额
        /// </summary>
        public decimal PayDeposit { get; set; }
        /// <summary>
        /// 收入月底结算金额
        /// </summary>
        public decimal EarnMonthEndPrice { get; set; }
        /// <summary>
        /// 收入月卡金额
        /// </summary>
        public decimal EarnWaterCardPrice { get; set; }
        /// <summary>
        /// 纪录日期
        /// </summary>
        public DateTime VisitDate { get; set; }
    }
}
