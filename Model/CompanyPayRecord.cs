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
}
