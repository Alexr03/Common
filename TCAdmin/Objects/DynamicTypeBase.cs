using System;
using TCAdmin.SDK.Objects;

namespace Alexr03.Common.TCAdmin.Objects
{
    public class DynamicTypeBase : ObjectBase
    {
        private DynamicTypeBase()
        {
            this.UseApplicationDataField = true;
        }

        public DynamicTypeBase(string tableName, string moduleId) : this()
        {
            this.TableName = tableName;
            this.KeyColumns = new[] {"id", "moduleId"};
            this.SetValue("id", -1);
            this.SetValue("moduleId", moduleId);
            this.ValidateKeys();
        }

        public int Id
        {
            get => this.GetIntegerValue("id");
            set => this.SetValue("id", value);
        }

        public string ModuleId
        {
            get => this.GetStringValue("moduleId");
            set => this.SetValue("moduleId", value);
        }

        protected string TypeName
        {
            get => this.GetStringValue("typeName");
            set => this.SetValue("typeName", value);
        }

        public Type Type => Type.GetType(TypeName);

        protected string ConfigurationName
        {
            get => this.GetStringValue("configurationName");
            set => this.SetValue("configurationName", value);
        }

        public object Create(object args = null)
        {
            return Create<object>(args);
        }

        public T Create<T>(object args = null)
        {
            if (args == null)
            {
                return (T) Activator.CreateInstance(Type);
            }

            return (T) Activator.CreateInstance(Type, args);
        }

        public ModuleConfiguration Configuration => ModuleConfiguration.GetModuleConfiguration(this.ModuleId, ConfigurationName);

        public virtual T GetConfiguration<T>() where T : new()
        {
            return ModuleConfiguration.GetModuleConfiguration(ModuleId, Configuration.Type.Name, typeof(T)).GetConfiguration<T>();
        }

        public virtual void SetConfiguration<T>(T config) where T : new()
        {
            var moduleConfiguration = ModuleConfiguration.GetModuleConfiguration(ModuleId, Configuration.Type.Name);
            moduleConfiguration.SetConfiguration(config);
        }
    }
}