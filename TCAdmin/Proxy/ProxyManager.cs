using System.Collections.Generic;
using System.Linq;
using TCAdmin.SDK;
using TCAdmin.SDK.Proxies;

namespace Alexr03.Common.TCAdmin.Proxy
{
    public static class ProxyManager
    {
        public static readonly List<CommandProxy> CommandProxies = new List<CommandProxy>();
        
        public static void AddProxy(this CommandProxy commandProxy)
        {
            CommandProxies.Add(commandProxy);
        }

        public static void RemoveProxy(this CommandProxy commandProxy)
        {
            CommandProxies.Remove(commandProxy);
        }

        public static void RegisterProxies()
        {
            foreach (var commandProxy in CommandProxies)
            {
                commandProxy.RegisterProxy();
            }
        }
        
        public static void RegisterProxy(this CommandProxy commandProxy)
        {
            if (!CommandProxies.Contains(commandProxy))
            {
                CommandProxies.Add(commandProxy);
            }
            
            AppDomainManager.RegisterProxyCommand(commandProxy);
        }
        
        public static void UnRegisterProxies()
        {
            foreach (var commandProxy in CommandProxies)
            {
                if (CommandProxies.Contains(commandProxy))
                {
                    CommandProxies.Remove(commandProxy);
                }
                UnRegisterProxy(commandProxy.CommandName);
            }
        }

        public static void UnRegisterProxy(string commandName)
        {
            if (CommandProxies.Any(x => x.CommandName == commandName))
            {
                CommandProxies.RemoveAll(x => x.CommandName == commandName);
            }
            AppDomainManager.UnregisterProxyCommand(commandName);
        }
    }
}