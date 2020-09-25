using System;
using System.Collections.Generic;
using System.Linq;
using Alexr03.Common.TCAdmin.Extensions;
using Newtonsoft.Json;
using TCAdmin.Interfaces.Database;
using TCAdmin.SDK.Objects;

namespace Alexr03.Common.TCAdmin.Objects
{
    public class ModuleConfiguration : ObjectBase
    {
        public const string ConfigurationViewKey = "AR_COMMON:ConfigurationView";
        
        public ModuleConfiguration()
        {
            this.TableName = "ar_common_configurations";
            this.KeyColumns = new[] {"id"};
            this.SetValue("id", -1);
            this.SetValue("contents", "{}");
            this.UseApplicationDataField = true;
        }

        public int Id
        {
            get => this.GetIntegerValue("id");
            private set => this.SetValue("id", value);
        }
        
        public string ConfigName
        {
            get => this.GetStringValue("configName");
            private set => this.SetValue("configName", value);
        }
        
        public string ModuleId
        {
            get => this.GetStringValue("moduleId");
            private set => this.SetValue("moduleId", value);
        }

        private string Contents
        {
            get => this.GetStringValue("contents");
            set => this.SetValue("contents", value);
        }

        private string TypeName
        {
            get => this.GetStringValue("configTypeName");
            set => this.SetValue("configTypeName", value);
        }

        public Type Type => Type.GetType(TypeName);
        
        public bool HasView() => View != null;

        public virtual string View
        {
            get => this.AppData.HasValueAndSet(ConfigurationViewKey)
                ? this.AppData[ConfigurationViewKey].ToString()
                : null;
            set => this.AppData[ConfigurationViewKey] = value;
        }

        public T GetConfiguration<T>()
        {
            // if (string.IsNullOrEmpty(Contents) || Contents == "{}")
            // {
            //     throw new Exception("Contents is empty. Cannot parse a empty/null value");
            // }
            return JsonConvert.DeserializeObject<T>(Contents);
        }

        public void SetConfiguration(object config)
        {
            var serializeObject = JsonConvert.SerializeObject(config);
            TypeName = $"{config.GetType()}, {config.GetType().Assembly.GetName().Name}";
            Contents = serializeObject;
            this.Save();
        }

        public static ModuleConfiguration GetModuleConfiguration(string moduleId, string configName, Type type = null)
        {
            var whereList = new WhereList
            {
                {nameof(moduleId), moduleId},
                {nameof(configName), configName},
            };

            var moduleConfigurations = new ModuleConfiguration().GetObjectList(whereList).Cast<ModuleConfiguration>().ToList();
            if (moduleConfigurations.Any())
            {
                var moduleConfiguration = moduleConfigurations[0];
                if (string.IsNullOrEmpty(moduleConfiguration.TypeName) && type != null)
                {
                    moduleConfiguration.SetConfiguration(Activator.CreateInstance(type));
                    moduleConfiguration.Save();
                }

                return moduleConfiguration;
            }

            if (type == null) return null;
            
            var newConfig = new ModuleConfiguration
            {
                ModuleId = moduleId,
                ConfigName = configName,
                TypeName = $"{type}, {type.Assembly.GetName().Name}"
            };
            newConfig.GenerateKey();
            newConfig.Save();
            newConfig.SetConfiguration(Activator.CreateInstance(type));

            return newConfig;
        }
        
        public static List<ModuleConfiguration> GetModuleConfigurations(string moduleId)
        {
            var whereList = new WhereList
            {
                {nameof(moduleId), moduleId},
            };

            var moduleConfigurations = new ModuleConfiguration().GetObjectList(whereList).Cast<ModuleConfiguration>().ToList();
            return moduleConfigurations;
        }
    }
}