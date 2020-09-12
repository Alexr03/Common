using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using TCAdmin.Interfaces.Logging;
using TCAdmin.SDK;
using TCAdmin.SDK.Misc;
using TCAdmin.SDK.Web.References.ModuleApiGateway;
using Server = TCAdmin.GameHosting.SDK.Objects.Server;

namespace Alexr03.Common.TCAdmin.Proxy.Requests
{
    public static class ProxyRequest
    {
        public static T Perform<T>(string commandName,
            object arguments, out CommandResponse commandResponse, bool waitForResponse = true,
            ProxyRequestType requestType = ProxyRequestType.Xml, JsonSerializerSettings settings = null)
        {
            try
            {
                var server = Server.GetServerFromCache(1);
                commandResponse = new CommandResponse();
                if (server.ModuleApiGateway.ExecuteModuleCommand(commandName, arguments, ref commandResponse,
                    waitForResponse))
                {
                    switch (requestType)
                    {
                        case ProxyRequestType.Xml:
                        {
                            var xmlToObject = (T) ObjectXml.XmlToObject(commandResponse.Response.ToString(), typeof(T));
                            return xmlToObject;
                        }
                        case ProxyRequestType.Json:
                            if (settings == null)
                            {
                                settings = Utilities.JsonSerializerSettings;
                            }

                            return JsonConvert.DeserializeObject<T>(commandResponse.Response.ToString(), settings);
                        default:
                            throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
                    }
                }
                
                throw new Exception("Proxy command execution failed.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                commandResponse = new CommandResponse {SerializedException = e.Message};
                return Activator.CreateInstance<T>();
            }
        }
    }

    public enum ProxyRequestType
    {
        Xml,
        Json
    }
}