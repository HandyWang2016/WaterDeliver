using System;

namespace Model
{
    public class Customer
    {
        public string Id { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 支付方式ID
        /// </summary>
        public PayTypeEnum PayType { get; set; }
        /// <summary>
        /// 下次送水日期
        /// </summary>
        public DateTime NextDate { get; set; }
        /// <summary>
        /// 是否接收通知
        /// </summary>
        public int NotifyFlag { get; set; }
    }
}