﻿using System.Collections.Generic;
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
            if (!CommandProxies.Contains(commandProxy))
            {
                CommandProxies.Add(commandProxy);
            }
        }

        public static void RemoveProxy(this CommandProxy commandProxy)
        {
            if (CommandProxies.Contains(commandProxy))
            {
                CommandProxies.Remove(commandProxy);
            }
        }
        
        public static void RemoveProxy(this string commandName)
        {
            if (CommandProxies.Any(x => x.CommandName == commandName))
            {
                CommandProxies.RemoveAll(x => x.CommandName == commandName);
            }
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
            AppDomainManager.RegisterProxyCommand(commandProxy);
            AddProxy(commandProxy);
        }
        
        public static void UnRegisterProxies()
        {
            foreach (var commandProxy in CommandProxies.ToList())
            {
                UnRegisterProxy(commandProxy.CommandName);
            }
            
            CommandProxies.Clear();
        }

        public static void UnRegisterProxy(string commandName)
        {
            AppDomainManager.UnregisterProxyCommand(commandName);
            RemoveProxy(commandName);
        }
    }
}