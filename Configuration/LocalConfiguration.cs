﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using TCAdmin.SDK;

namespace Alexr03.Common.Configuration
{
    public class LocalConfiguration<T> : ConfigurationProvider<T>
    {
        private const string ConfigBaseLocation = "./Components/{0}/Config/";
        private readonly string _configLocation;
        private readonly Type _type = typeof(T);
        private readonly string _assemblyName = typeof(T).Assembly.GetName().Name;

        public LocalConfiguration() : this(typeof(T).Name)
        {
        }

        public LocalConfiguration(string configName) : base(configName)
        {
            ConfigName = !ConfigName.EndsWith(".json") ? ConfigName + ".json" : ConfigName;
            _configLocation =
                Path.Combine(
                    ConfigBaseLocation.Replace("{0}",
                        Path.Combine(_assemblyName, _type.Namespace?.Replace(_assemblyName, "") ?? "")
                            .Replace(".", "")), ConfigName);
        }

        public override T GetConfiguration()
        {
            try
            {
                var fileInfo = new FileInfo(_configLocation);
                fileInfo.Directory?.Create();
                if (!fileInfo.Exists)
                {
                    if (!GenerateIfNonExisting) return GetTObject();
                    Log.Information($"Config '{fileInfo.Name}' does not exist. Auto generating.");
                    var defaultT = GetTObject();
                    SetConfiguration(defaultT);
                    return defaultT;
                }

                var fileContents = File.ReadAllText(fileInfo.FullName);
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
            try
            {
                var serializedObject = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(_configLocation, serializedObject, Encoding.Default);

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