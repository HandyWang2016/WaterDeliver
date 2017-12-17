using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class StaffCustomer
    {
        public int Id { get; set; }
        /// <summary>
        /// 员工Id
        /// </summary>
        public int StaffId { get; set; }
        /// <summary>
        /// 客户Id
        /// </summary>
        public int CustomerId { get; set; }
    }
}
