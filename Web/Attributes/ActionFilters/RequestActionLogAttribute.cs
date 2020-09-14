using System;
using System.Web.Mvc;
using TCAdmin.SDK;

namespace Alexr03.Common.Web.Attributes.ActionFilters
{
    public class RequestActionLogAttribute : ActionFilterAttribute
    {
        private readonly string _logName;
        private readonly bool _debug;

        private const string RequestReceived = "|------------------------Request Received------------------------|";
        private const string Separator = "|----------------------------------------------------------------|";

        public RequestActionLogAttribute(string logName = "Alexr03.Common", bool debug = false)
        {
            _logName = logName;
            _debug = debug;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Log("OnActionExecuting", filterContext);
        }

        private void Log(string eventType, ActionExecutingContext context)
        {
            var routeData = context.RouteData;
            var controllerName = routeData.Values["controller"].ToString();
            var actionName = routeData.Values["action"].ToString();
            var message = $"Controller: {controllerName}\nAction: {actionName}\nEvent Type: {eventType}";
            LogManager.WriteToLog(controllerName, message, true, _logName);
            if (_debug)
            {
                Console.WriteLine(RequestReceived);
                Console.WriteLine(message);
            }

            foreach (var contextActionParameter in context.ActionParameters)
            {
                var parameterMessage = $"|--- {contextActionParameter.Key} = {contextActionParameter.Value}";
                LogManager.WriteToLog(controllerName, parameterMessage, true, _logName);
                if (_debug)
                {
                    Console.WriteLine(parameterMessage);
                }
            }

            LogManager.WriteToLog(controllerName, Separator, true, _logName);
            if (_debug)
            {
                Console.WriteLine(Separator);
            }
        }
    }
}