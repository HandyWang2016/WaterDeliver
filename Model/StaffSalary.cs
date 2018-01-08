﻿namespace Model
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
        public string SalaryMonth { get; set; }
        /// <summary>
        /// 薪资金额
        /// </summary>
        public double Salary { get; set; }
    }
}