using System;
using Alexr03.Common.Configuration;
using Newtonsoft.Json;

namespace Alexr03.Common.TCAdmin.Configuration
{
    public class DatabaseConfiguration<T> : ConfigurationProvider<T>
    {
        protected override string ProviderName { get; set; } = "Database";

        public DatabaseConfiguration()
        {
        }

        public DatabaseConfiguration(string configName) : base(configName)
        {
            this.ConfigName = ConfigName.Replace(".json", "");
        }

        public override T GetConfiguration()
        {
            //todo generate if non existent.
            var configuration = global::TCAdmin.SDK.Utility.GetDatabaseValue(ConfigName);
            return !string.IsNullOrEmpty(configuration)
                ? JsonConvert.DeserializeObject<T>(configuration)
                : GetTObject();
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