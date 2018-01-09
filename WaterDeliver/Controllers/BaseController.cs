using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers
{
    public class BaseController : Controller
    {
        private static string staffId;

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

        /// <summary>
        /// 分页显示的条目数
        /// </summary>
        private static int _pageSize;

        public int PageSize()
        {
            if (_pageSize == 0)
            {
                _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            }
            return _pageSize;
        }

        /// <summary>
        /// 送1桶水的提成
        /// </summary>
        private static double _commission;

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