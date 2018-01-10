using System.Collections.Generic;

namespace Model
{
    public class Factory
    {
        public string Id { get; set; }
        /// <summary>
        /// 厂家名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 其他信息
        /// </summary>
        public string Describe { get; set; }
    }

    /// <summary>
    /// 水厂月库存统计
    /// </summary>
    public class FactoryStock
    {
        public string FactoryName { get; set; }
        public string ProductName { get; set; }
        public int BucketStock { get; set; }
        public int EmptyBucketStock { get; set; }
        public string TransMonth { get; set; }
    }

    public class FactoryTrans
    {
        /// <summary>
        /// 水厂名称
        /// </summary>
        public string FactoryName { get; set; }
        /// <summary>
        /// 涉及水厂的交易类别
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public double TransSum { get; set; }
        /// <summary>
        /// 是否支出类型
        /// </summary>
        public bool IsPayType { get; set; }
        /// <summary>
        /// 交易月份 eg:2017-12
        /// </summary>
        public string TransMonth { get; set; }
    }

    public class FactorySumaryViewModel
    {
        public List<FactoryStock> FactoryStock { get; set; }
        public List<FactoryTrans> FactoryTrans { get; set; }
    }
}