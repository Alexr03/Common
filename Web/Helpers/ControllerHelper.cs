using System.Web.Mvc;
using System.Web.SessionState;

namespace Alexr03.Common.Web.Helpers
{
    public static class ControllerHelper
    {
        public static void AddTableNameToSession(this ControllerBase controllerBase, string tableName)
        {
            controllerBase.ControllerContext.HttpContext.SetSessionStateBehavior(SessionStateBehavior.Required);
            controllerBase.ControllerContext.HttpContext.Session["tableName"] = tableName;
        }
    }
}