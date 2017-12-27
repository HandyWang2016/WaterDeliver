using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WaterDeliver.Controllers
{
    public class BaseController : Controller
    {
        private string staffId;

        public string GetStaffId()
        {
            if (string.IsNullOrEmpty(staffId))
            {
                return Session["staffId"] == null ? "" : Session["staffId"].ToString();
            }
            else
            {
                return staffId;
            }
        }
    }
}