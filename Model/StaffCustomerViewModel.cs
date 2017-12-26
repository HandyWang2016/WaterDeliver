using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// 创建ViewModel,组装Staff,Customer,StaffCustomer实体集合
    /// </summary>
    public class StaffCustomerViewModel
    {
        public List<Staff> Staffs { get; set; }
        public List<Customer> Customers { get; set; }
        public List<StaffCustomer> StaffCustomers { get; set; }
    }
}