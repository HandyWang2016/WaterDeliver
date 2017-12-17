using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WaterCardType
    {
        public int Id { get; set; }
        /// <summary>
        /// 水卡金额
        /// </summary>
        public decimal CardPrice { get; set; }
        /// <summary>
        /// 包含桶装水数量
        /// </summary>
        public int BucketAmount { get; set; }
    }
}
