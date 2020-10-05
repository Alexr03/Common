using System;
using System.Net;
using System.Web.Mvc;
using Alexr03.Common.Web.HttpResponses;

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

        public static ActionResult SendError(this ControllerBase controllerBase, string message, HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
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
    }
}