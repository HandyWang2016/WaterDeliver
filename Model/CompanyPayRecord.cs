using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class CompanyPayRecord
    {
        public int Id { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public CompanyPayTypeEnum PayType { get; set; }
        /// <summary>
        /// 支出金额
        /// </summary>
        public decimal PaySum { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}
