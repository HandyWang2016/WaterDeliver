using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum CompanyPayTypeEnum
    {
        /// <summary>
        /// 厂家买空桶支出
        /// </summary>
        EmptyBucket = 0,

        /// <summary>
        /// 买桶装水
        /// </summary>
        BucketWater = 1,

        /// <summary>
        /// 买饮水机
        /// </summary>
        WaterDrinker = 2,

        /// <summary>
        /// 员工提成
        /// </summary>
        StaffTicheng = 3,

        /// <summary>
        /// 税额
        /// </summary>
        Tax = 4,

        /// <summary>
        /// 罚款
        /// </summary>
        Penalty = 5,

        /// <summary>
        /// 租金
        /// </summary>
        Rent = 6,

        /// <summary>
        /// 其他
        /// </summary>
        OtherPay = 7
    }
}
