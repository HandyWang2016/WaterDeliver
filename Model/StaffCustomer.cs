using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class StaffCustomer
    {
        public string Id { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public string StaffId { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public string CustomerId { get; set; }
    }
}
