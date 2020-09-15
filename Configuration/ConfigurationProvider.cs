using System;

namespace Alexr03.Common.Configuration
{
    public abstract class ConfigurationProvider<T>
    {
        protected abstract string ProviderName { get; set; }

        protected string ConfigName { get; set; }
        public bool GenerateIfNonExisting { get; set; }

        protected ConfigurationProvider()
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
            if (typeof(T).IsValueType || typeof(T) == typeof(string)) return default;
            return (T) Activator.CreateInstance(typeof(T));
        }
    }
}