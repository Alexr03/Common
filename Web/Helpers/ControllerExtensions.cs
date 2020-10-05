using System.Net;
using System.Web.Mvc;
using Alexr03.Common.Web.HttpResponses;

namespace Alexr03.Common.Web.Helpers
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
        
        public static ActionResult SendError(this ControllerBase controllerBase, string message)
        {
            return new JsonNetResult(new
            {
                Message = $"<strong>Error:</strong> {message}"
            }, HttpStatusCode.BadRequest);
        }
    }
}