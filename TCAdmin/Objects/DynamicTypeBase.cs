using System;
using Alexr03.Common.TCAdmin.Configuration;
using Alexr03.Common.TCAdmin.Extensions;
using TCAdmin.SDK.Objects;

namespace Alexr03.Common.TCAdmin.Objects
{
    public class DynamicTypeBase : ObjectBase
    {
        public const string ConfigurationViewKey = "AR_COMMON:ConfigurationView";
        
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

        public Type ConfigurationType => Type.GetType(ConfigurationTypeName);

        protected string TypeName
        {
            get => this.GetStringValue("typeName");
            set => this.SetValue("typeName", value);
        }

        public bool HasConfigurationView() => ConfigurationView != null;

        public virtual string ConfigurationView
        {
            get => this.AppData.HasValueAndSet(ConfigurationViewKey) ? this.AppData["ConfigurationViewKey"].ToString() : null;
            set => this.AppData[ConfigurationViewKey] = value;
        }

        protected string ConfigurationTypeName
        {
            get => this.GetStringValue("configurationTypeName");
            set => this.SetValue("configurationTypeName", value);
        }

        public T Create<T>(object args)
        {
            return (T)Activator.CreateInstance(ConfigurationType, args);
        }

        public virtual T GetConfiguration<T>() where T : new()
        {
            var type = ConfigurationType;
            var databaseConfiguration = new DatabaseConfiguration<T>("DTB-" + type?.Name).GetConfiguration();

            return databaseConfiguration;
        }
        
        public virtual void SetConfiguration<T>(T config) where T : new()
        {
            var type = Type.GetType(ConfigurationTypeName);
            new DatabaseConfiguration<T>("DTB-" + type?.Name).SetConfiguration(config);
        }
    }
}