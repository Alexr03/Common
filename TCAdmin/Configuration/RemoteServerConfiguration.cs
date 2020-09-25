using System;
using System.IO;
using System.Text;
using Alexr03.Common.Configuration;
using Newtonsoft.Json;
using Serilog;
using TCAdmin.GameHosting.SDK.Objects;

namespace Alexr03.Common.TCAdmin.Configuration
{
    public class RemoteServerConfiguration<T> : ConfigurationProvider<T>
    {
        private const string ConfigBaseLocation = "{0}/Components/{1}/Config/";
        private int ServerId = 1;
        private readonly string _configLocation;
        private readonly Type _type = typeof(T);
        private readonly string _assemblyName = typeof(T).Assembly.GetName().Name;

        public RemoteServerConfiguration() : this(1)
        {
        }

        public RemoteServerConfiguration(int serverId) : this(typeof(T).Name)
        {
            this.ServerId = serverId;
        }

        public RemoteServerConfiguration(string configName) : base(configName)
        {
            var server = new Server(ServerId);
            ConfigName = !ConfigName.EndsWith(".json") ? ConfigName + ".json" : ConfigName;
            var replace = global::TCAdmin.SDK.Misc.FileSystem.CombinePath(server.OperatingSystem,
                server.ServerUtilitiesService.GetMonitorDirectory(), "Components", _assemblyName,
                _type.Namespace?.Replace(_assemblyName, "")).Replace(".", "");
            _configLocation =
                global::TCAdmin.SDK.Misc.FileSystem.CombinePath(server.OperatingSystem,
                    replace, ConfigName);
        }

        public override T GetConfiguration()
        {
            var server = new Server(ServerId);
            var fileSystem = server.FileSystemService;
            try
            {
                var fileInfo = new FileInfo(_configLocation);
                var readFileExtended = fileSystem.ReadFileExtended(fileInfo.FullName.Replace(fileInfo.Name, ""), fileInfo.Name, 1000000000);
                fileSystem.CreateDirectory(fileInfo.Directory?.FullName);
                if (!readFileExtended.Exists)
                {
                    Log.Information($"Config '{fileInfo.Name}' does not exist. Auto generating.");
                    var defaultT = GetTObject();
                    SetConfiguration(defaultT);
                    return defaultT;
                }

                var fileContents = Encoding.Default.GetString(readFileExtended.Contents);
                var deserializeObject = JsonConvert.DeserializeObject<T>(fileContents);
                return deserializeObject;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unable to parse {ConfigName} to object {GetType().FullName}. Please ensure that the JSON is correct.");
            }

            return GetTObject();
        }

        public override bool SetConfiguration(T config)
        {
            var server = new Server(ServerId);
            var fileSystem = server.FileSystemService;
            try
            {
                var serializedObject = JsonConvert.SerializeObject(config, Formatting.Indented);
                fileSystem.CreateTextFile(_configLocation, Encoding.Default.GetBytes(serializedObject));

                Log.Information("Saved new Configuration: " + ConfigName);
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Unable to save configuration because {e.Message}");
                return false;
            }
        }
    }
}