using System;
using Newtonsoft.Json;

namespace Alexr03.Common.TCAdmin.Proxy.Requests
{
    [Serializable]
    public abstract class ProxyRequest
    {
        [JsonIgnore]
        public abstract string CommandName { get; }

        public abstract object Execute(object arguments);
    }
}