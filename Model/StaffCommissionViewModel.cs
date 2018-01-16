namespace Model
{
    /// <summary>
    /// 员工提成(送水桶数*每一桶提成)
    /// </summary>
    public class StaffCommissionViewModel
    {
        public string StaffName { get; set; }
        public int BucketCount { get; set; }
        public double Comission { get; set; }
    }
}