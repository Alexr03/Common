using System;
using System.Web.Mvc;

namespace Alexr03.Common.Web.Helpers
{
    public static class FormCollectionExtensions
    {
        public static T Parse<T>(this FormCollection formCollection, ControllerContext controllerContext, bool useEmptyPrefix = false)
        {
            return (T)Parse(formCollection, controllerContext, typeof(T), useEmptyPrefix);
        }
        
        public static T Parse<T>(this FormCollection formCollection, ControllerContext controllerContext, string prefix, bool useEmptyPrefix = false)
        {
            return (T)Parse(formCollection, controllerContext, typeof(T), prefix, useEmptyPrefix);
        }

        public static object Parse(this FormCollection formCollection, ControllerContext controllerContext, Type configurationType, bool useEmptyPrefix = false)
        {
            return Parse(formCollection, controllerContext, configurationType, configurationType.Name, useEmptyPrefix);
        }
        
        public static object Parse(this FormCollection formCollection, ControllerContext controllerContext, Type configurationType, string prefix, bool useEmptyPrefix = false)
        {
            var metadataForType = ModelMetadataProviders.Current.GetMetadataForType(null, configurationType);
            var modelBindingContext = new ModelBindingContext
            {
                ModelName = prefix,
                ModelMetadata = metadataForType,
                ValueProvider = formCollection.ToValueProvider(),
                FallbackToEmptyPrefix = useEmptyPrefix
            };
            var binder = new DefaultModelBinder().BindModel(controllerContext, modelBindingContext);
            return binder;
        }
    }
}