using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Staff
    {
        public string Id { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string StaffPhone { get; set; }
        /// <summary>
        /// 其他个人信息
        /// </summary>
        public string Describe { get; set; }
    }
}
