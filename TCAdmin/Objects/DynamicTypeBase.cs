using System;
using Alexr03.Common.TCAdmin.Configuration;
using Alexr03.Common.TCAdmin.Extensions;
using TCAdmin.SDK.Objects;

namespace Alexr03.Common.TCAdmin.Objects
{
    public class DynamicTypeBase : ObjectBase
    {
        public const string ConfigurationViewKey = "AR_COMMON:ConfigurationView";

        public string ConfigurationPrefix { get; set; } = "DTB-";
        
        private DynamicTypeBase()
        {
            this.UseApplicationDataField = true;
        }

        public DynamicTypeBase(string tableName) : this()
        {
            this.TableName = tableName;
            this.KeyColumns = new[] {"id"};
            this.SetValue("id", -1);
            this.ValidateKeys();
        }

        public int Id
        {
            get => this.GetIntegerValue("id");
            set => this.SetValue("id", value);
        }

        protected string TypeName
        {
            get => this.GetStringValue("typeName");
            set => this.SetValue("typeName", value);
        }
        
        public Type Type => Type.GetType(TypeName);

        public bool HasConfigurationView() => ConfigurationView != null;

        public virtual string ConfigurationView
        {
            get => this.AppData.HasValueAndSet(ConfigurationViewKey) ? this.AppData[ConfigurationViewKey].ToString() : null;
            set => this.AppData[ConfigurationViewKey] = value;
        }

        protected string ConfigurationTypeName
        {
            get => this.GetStringValue("configurationTypeName");
            set => this.SetValue("configurationTypeName", value);
        }

        public Type ConfigurationType => Type.GetType(ConfigurationTypeName);

        public object Create(object args = null)
        {
            return Create<object>(args);
        }

        public T Create<T>(object args = null)
        {
            if (args == null)
            {
                return (T)Activator.CreateInstance(Type);
            }
            return (T)Activator.CreateInstance(Type, args);
        }

        public virtual T GetConfiguration<T>() where T : new()
        {
            var type = ConfigurationType;
            var databaseConfiguration = new DatabaseConfiguration<T>(ConfigurationPrefix + type?.Name).GetConfiguration();

            return databaseConfiguration;
        }
        
        public virtual void SetConfiguration<T>(T config) where T : new()
        {
            var type = Type.GetType(ConfigurationTypeName);
            new DatabaseConfiguration<T>(ConfigurationPrefix + type?.Name).SetConfiguration(config);
        }
    }
}