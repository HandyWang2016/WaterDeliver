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
        private static int _pageSize;

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