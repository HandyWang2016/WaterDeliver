using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Products
    {
        public string Id { get; set; }
        /// <summary>
        /// 产品所属水厂
        /// </summary>
        public string FactoryId { get; set; }
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
        public double CostPrice { get; set; }
        /// <summary>
        /// 空桶库存
        /// </summary>
        public int BucketStockRemain { get; set; }
        /// <summary>
        /// 空桶成本
        /// </summary>
        public double BucketCostPrice { get; set; }
        /// <summary>
        /// 销售价格
        /// </summary>
        public double SalePrice { get; set; }
        /// <summary>
        /// 库存更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }

    public class ProductFac : Products
    {
        public string FactoryName { get; set; }
    }
}
