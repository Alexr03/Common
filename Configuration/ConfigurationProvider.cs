using System;
using Alexr03.Common.Logging;

namespace Alexr03.Common.Configuration
{
    public abstract class ConfigurationProvider<T>
    {
        private readonly Logger _logger = Logger.Create<ConfigurationProvider<T>>();

        protected string ConfigName { get; set; }

        protected ConfigurationProvider() : this(typeof(T).Name)
        {
        }

        protected ConfigurationProvider(string configName)
        {
            ConfigName = configName;
        }

        public abstract T GetConfiguration();
        public abstract bool SetConfiguration(T config);

        protected T GetTObject()
        {
            _logger.Information("Generated default config for " + typeof(T).Name);
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) return default;
            return (T) Activator.CreateInstance(typeof(T));
        }
    }
}