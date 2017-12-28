using System;

namespace Model
{
    /// <summary>
    /// 日常信息查询实体
    /// </summary>
    public class DailyQueryMod
    {
        public string StaffId { get; set; }
        public string CustomerId { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
    }
}