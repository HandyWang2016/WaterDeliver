using System;

namespace Model
{
    /// <summary>
    /// 公司消费记录查询实体
    /// </summary>
    public class CompanyQueryMod
    {
        public string StaffId { get; set; }
        public string PayTypeId { get; set; }
        public DateTime PayTimeBegin { get; set; }
        public DateTime PayTimeEnd { get; set; }

    }
}