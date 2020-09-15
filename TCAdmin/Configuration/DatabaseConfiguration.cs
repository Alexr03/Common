using System;
using Alexr03.Common.Configuration;
using Newtonsoft.Json;

namespace Alexr03.Common.TCAdmin.Configuration
{
    public class DatabaseConfiguration<T> : ConfigurationProvider<T>
    {
        protected override string ProviderName { get; set; } = "Database";

        public DatabaseConfiguration() : this(typeof(T).Name)
        {
        }

        public DatabaseConfiguration(string configName) : base(configName)
        {
            this.ConfigName = ConfigName.Replace(".json", "");
            if (ConfigName.Length <= 25) return;
            Console.WriteLine("Config Name is more than 25 characters, this is unsupported for tc_info. Performed Substring.");
            this.ConfigName = this.ConfigName.Substring(0, 25);
        }

        public override T GetConfiguration()
        {
            var configuration = global::TCAdmin.SDK.Utility.GetDatabaseValue(ConfigName);
            if (!string.IsNullOrEmpty(configuration)) return JsonConvert.DeserializeObject<T>(configuration);
            
            var tObject = GetTObject();
            if (GenerateIfNonExisting)
            {
                this.SetConfiguration(tObject);
            }
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