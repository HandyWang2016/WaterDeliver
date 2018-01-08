namespace Model
{
    /// <summary>
    /// 月底账目汇总实体
    /// </summary>
    public class MonthEndSummary
    {
        /// <summary>
        /// 员工总收入
        /// </summary>
        public double StaffEarn { get; set; }
        /// <summary>
        /// 员工总支出
        /// </summary>
        public double StaffPay { get; set; }
        /// <summary>
        /// 公司总收入
        /// </summary>
        public double CompanyEarn { get; set; }
        /// <summary>
        /// 公司总支出
        /// </summary>
        public double CompanyPay { get; set; }
        /// <summary>
        /// 月底总盈利：员工收入+公司收入-员工支出-公司支出
        /// </summary>
        public double MonthEndEarn { get; set; }
        /// <summary>
        /// 结算的年份月份
        /// </summary>
        public string YearMonth { get; set; }
    }
}