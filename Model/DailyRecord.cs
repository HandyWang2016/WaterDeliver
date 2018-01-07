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
        /// 收入结算金额
        /// </summary>
        public double EarnMonthEndPrice { get; set; }
        /// <summary>
        /// 纪录日期
        /// </summary>
        public DateTime VisitDate { get; set; }
    }

    public class DailyRecordShow : DailyRecord
    {
        public string StaffName { get; set; }
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
    }


}
