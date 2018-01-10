using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers.Admin
{
    public class Admin_BaseController : Controller
    {
        /// <summary>
        /// 分页条数
        /// </summary>
        private static int _pageSize;
        /// <summary>
        /// 员工薪资支出类型
        /// </summary>
        protected const string SalaryPayType = "50c8d301097facb82b660000";
        /// <summary>
        /// 进水支出类型
        /// </summary>
        protected const string BucketPayType = "50c8d301097facb82b670000";

        public int PageSize()
        {
            if (_pageSize == 0)
            {
                _pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            }
            return _pageSize;
        }
    }
}