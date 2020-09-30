using System;
using System.Web.Mvc;

namespace Alexr03.Common.Web.Attributes.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisallowDirect : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                filterContext.HttpContext.Request.Url?.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}