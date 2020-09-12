using System;
using TCAdmin.SDK.Misc;
using TCAdmin.SDK.Web.References.ModuleApiGateway;
using Server = TCAdmin.GameHosting.SDK.Objects.Server;

namespace Alexr03.Common.TCAdmin.Proxy.Requests
{
    public static class ProxyRequest
    {
        public static T Perform<T>(string commandName,
            object arguments, out CommandResponse commandResponse, bool waitForResponse = true)
        {
            try
            {
                var server = Server.GetServerFromCache(1);
                commandResponse = new CommandResponse();
                server.ModuleApiGateway.ExecuteModuleCommand(commandName, arguments, ref commandResponse,
                    waitForResponse);

                var xmlToObject = (T) ObjectXml.XmlToObject(commandResponse.Response.ToString(), typeof(T));
                return xmlToObject;
            }
            catch (Exception e)
            {
                commandResponse = new CommandResponse {SerializedException = e.Message};
                return Activator.CreateInstance<T>();
            }
        }
    }
}