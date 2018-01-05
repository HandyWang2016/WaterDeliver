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
        /// 厂家所有产品
        /// </summary>
        public List<Products> Products { get; set; }
    }
}