using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CompanyPayRecord
    {
        public string Id { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayTypeId { get; set; }

        /// <summary>
        /// 费用关系员工ID
        /// </summary>
        public string StaffId { set; get; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public double PaySum { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }

    public class CompanyPayRecordDesc : CompanyPayRecord
    {
        /// <summary>
        /// 支付类型描述
        /// </summary>
        public string PayTypeDesc { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string StaffName { get; set; }
    }

    public class CompanyPayRecordViewModel
    {
        public List<CompanyPayRecord> CompanyPayRecord { get; set; }
        public List<CompanyPayRecordDesc> CompanyPayRecordDesc { get; set; }
    }
}
