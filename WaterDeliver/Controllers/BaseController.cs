using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = @"log4net.config", Watch = true)]
namespace WaterDeliver.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static string staffId;
        /// <summary>
        /// 分页显示的条目数
        /// </summary>
        private static int _pageSize;
        /// <summary>
        /// 送1桶水的提成
        /// </summary>
        private static double _commission;

        /// <summary>
        /// 员工薪资支出类型
        /// </summary>
        protected const string SalaryPayType = "50c8d301097facb82b660000";
        /// <summary>
        /// 进水支出类型
        /// </summary>
        protected const string BucketPayType = "50c8d301097facb82b670000";

        protected string GetStaffId()
        {
            if (string.IsNullOrEmpty(staffId))
                staffId = GetStaffIds();
            return staffId;
        }

        /// <summary>
        /// 获取登录用户id
        /// </summary>
        /// <returns></returns>
        private string GetStaffIds()
        {
            return Session?["staffId"]?.ToString() ?? "";
        }
        
        public int PageSize()
        {
            if (_pageSize == 0)
            {
                _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            }
            return _pageSize;
        }

        public double Commission()
        {
            if (_commission == 0)
            {
                _commission = double.Parse(ConfigurationManager.AppSettings["Commission"]);
            }
            return _commission;
        }
    }
}