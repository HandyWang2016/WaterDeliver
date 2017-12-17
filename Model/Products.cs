using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Products
    {
        public int Id { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 库存余量
        /// </summary>
        public int StockRemain { get; set; }
        /// <summary>
        /// 成本价格
        /// </summary>
        public decimal CostPrice { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public decimal SalePrice { get; set; }
        /// <summary>
        /// 库存更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
