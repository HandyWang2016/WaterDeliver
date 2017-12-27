using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WaterDeliver.Controllers.Common
{
    /// <summary>
    /// DataHandle 的摘要说明
    /// </summary>
    public class DataHandle : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string op = context.Request.Params["op"];
            string result = string.Empty;
            switch (op)
            {
                case "SetStaff":
                    SetStaff(context);
                    break;
            }
        }

        /// <summary>
        /// 设置登录用户
        /// </summary>
        /// <param name="context"></param>
        private void SetStaff(HttpContext context)
        {
            string staffId = context.Request.Params["staffId"];
            context.Session.Add("staffId", staffId);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}