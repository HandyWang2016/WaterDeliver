using System;
using System.Collections.Generic;
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
        ///     获取登录用户id
        /// </summary>
        /// <returns></returns>
        private string GetStaffIds()
        {
            return Session?["staffId"]?.ToString() ?? "";
        }
    }
}