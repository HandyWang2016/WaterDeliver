using System;

namespace Model
{
    /// <summary>
    /// 员工工资信息
    /// </summary>
    public class StaffSalary
    {
        public string Id { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public string StaffId { get; set; }
        /// <summary>
        /// 薪资月：eg:2017-12
        /// </summary>
        public DateTime SalaryMonth { get; set; }
        /// <summary>
        /// 员工提成：一桶水提2元
        /// </summary>
        public double Commission { get; set; }
        /// <summary>
        /// 薪资金额
        /// </summary>
        public double Salary { get; set; }
    }

    public class StaffSalaryDesc : StaffSalary
    {
        public string StaffName { get; set; }
        public double MonthIncome { get; set; }
    }
}