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
}