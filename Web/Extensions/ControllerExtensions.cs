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
        public static ActionResult SendSuccess(this ControllerBase controllerBase, string message)
        {
            return new JsonNetResult(new
            {
                Message = message
            });
        }

        public static ActionResult SendError(this ControllerBase controllerBase, string message,
            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
        {
            return new JsonNetResult(new
            {
                Message = $"<strong>Error:</strong> {message}"
            }, httpStatusCode);
        }

        public static ActionResult SendException(this ControllerBase controllerBase, Exception e)
        {
            return SendException(controllerBase, e, e.Message);
        }

        public static ActionResult SendException(this ControllerBase controllerBase, Exception e, string message)
        {
            return new JsonNetResult(new
            {
                Message = $"<strong>Error:</strong> {message}",
                Exception = e
            }, HttpStatusCode.InternalServerError);
        }

        public static void PrepareAjax(this ControllerBase controllerBase, string contentType = "application/json")
        {
            controllerBase.ControllerContext.HttpContext.Response.BufferOutput = false;
            controllerBase.ControllerContext.HttpContext.Response.ContentType = contentType;
        }

        public static void WriteAjaxMessage(this ControllerBase controllerBase, HtmlString message)
        {
            WriteAjaxMessage(controllerBase, message.ToString());
        }

        public static void WriteAjaxMessage(this ControllerBase controllerBase, HtmlString message, Logger logger,
            LogLevel level = LogLevel.Information)
        {
            WriteAjaxMessage(controllerBase, message.ToString(), logger, level);
        }

        public static void WriteAjaxMessage(this ControllerBase controllerBase, string message)
        {
            controllerBase.ControllerContext.HttpContext.Response.Clear();
            var serializeObject = JsonConvert.SerializeObject(new {Message = message});
            controllerBase.ControllerContext.HttpContext.Response.Write(serializeObject);
            controllerBase.ControllerContext.HttpContext.Response.Flush();
        }

        public static void WriteAjaxMessage(this ControllerBase controllerBase, string message, Logger logger,
            LogLevel level = LogLevel.Information)
        {
            WriteAjaxMessage(controllerBase, message);
            logger.LogMessage((LogEventLevel)level, message);
        }
    }
}