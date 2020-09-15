using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace Alexr03.Common.Configuration
{
    public class LocalConfiguration<T> : ConfigurationProvider<T>
    {
        private const string ConfigBaseLocation = "./Components/{0}/Config/";
        private readonly string _configLocation;
        private readonly string _assemblyName = typeof(T).Assembly.GetName().Name;
        
        protected override string ProviderName { get; set; } = "Local";

        public LocalConfiguration() : this(typeof(T).Name)
        {
            
        }

        public LocalConfiguration(string configName) : base(configName)
        {
            ConfigName = !ConfigName.EndsWith(".json") ? ConfigName + ".json" : ConfigName;
            this._configLocation = Path.Combine(ConfigBaseLocation.Replace("{0}", _assemblyName), ConfigName);
        }

        public override T GetConfiguration()
        {
            try
            {
                if (!Directory.Exists(ConfigBaseLocation))
                {
                    Directory.CreateDirectory(ConfigBaseLocation);
                }
                if (!File.Exists(_configLocation))
                {
                    if (!GenerateIfNonExisting) return GetTObject();
                    Console.WriteLine("Config does not exist. Auto generating.");
                    var defaultT = GetTObject();
                    SetConfiguration(defaultT);
                    return defaultT;
                }
                var fileContents = File.ReadAllText(_configLocation);
                var deserializeObject = JsonConvert.DeserializeObject<T>(fileContents);
                return deserializeObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"Unable to parse {ConfigName} to object {GetType().FullName}. Please ensure that the JSON is correct.");
                Console.WriteLine($"Message: {e.Message}");
            }

            return GetTObject();
        }

        public override bool SetConfiguration(T config)
        {
            try
            {
                var serializedObject = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configLocation, serializedObject, Encoding.Default);

                Console.WriteLine("Saved new Configuration: " + ConfigName);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable to save configuration because {e.Message}");
                return false;
            }
        }
    }
}