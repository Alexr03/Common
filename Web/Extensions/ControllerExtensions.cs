using System;
using System.Net;
using System.Web.Mvc;
using Alexr03.Common.Logging;
using Alexr03.Common.Web.HttpResponses;
using Alexr03.Common.Web.Models;
using Newtonsoft.Json;
using Serilog.Events;

namespace Alexr03.Common.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static void SetHtmlFieldPrefix(this ControllerBase controllerBase, string s)
        {
            controllerBase.ViewData.TemplateInfo = new TemplateInfo
            {
                HtmlFieldPrefix = s
            };
        }
    }
}