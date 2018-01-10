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
        /// 费用关系员工ID(如交罚款)
        /// </summary>
        public string StaffId { set; get; }
        /// <summary>
        /// 费用关联水厂ID(如押金交易，进水支出)
        /// </summary>
        public string FactoryId { get; set; }
        /// <summary>
        /// 交易类型是否是支付 true:支付 false:
        /// </summary>
        public bool IsPayType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public double TransSum { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime TransTime { get; set; }
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
