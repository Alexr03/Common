using System;
using Alexr03.Common.Configuration;
using Alexr03.Common.Logging;
using Newtonsoft.Json;

namespace Alexr03.Common.TCAdmin.Configuration
{
    public class DatabaseConfiguration<T> : ConfigurationProvider<T>
    {
        public DatabaseConfiguration() : this(typeof(T).Name)
        {
        }

        public DatabaseConfiguration(string configName) : base(configName)
        {
            var logger = Logger.Create<DatabaseConfiguration<T>>();
            ConfigName = ConfigName.Replace(".json", "");
            if (ConfigName.Length <= 25) return;
            logger.LogMessage(
                "Config Name is more than 25 characters, this is unsupported for tc_info. Performed Substring.");
            ConfigName = ConfigName.Substring(0, 25);
        }

        public override T GetConfiguration()
        {
            var configuration = global::TCAdmin.SDK.Utility.GetDatabaseValue(ConfigName);
            if (!string.IsNullOrEmpty(configuration)) return JsonConvert.DeserializeObject<T>(configuration);

            var tObject = GetTObject();
            if (GenerateIfNonExisting) SetConfiguration(tObject);
            return tObject;
        }

        public override bool SetConfiguration(T config)
        {
            try
            {
                var json = JsonConvert.SerializeObject(config);
                global::TCAdmin.SDK.Utility.SetDatabaseValue(ConfigName, json);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}