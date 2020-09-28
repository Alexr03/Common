﻿using System;
using System.Web.Mvc;

namespace Alexr03.Common.Web.Helpers
{
    public static class FormCollectionExtensions
    {
        public static T Parse<T>(this FormCollection formCollection, ControllerContext controllerContext, bool useEmptyPrefix = false)
        {
            return (T)Parse(formCollection, controllerContext, typeof(T), useEmptyPrefix);
        }

        public static object Parse(this FormCollection formCollection, ControllerContext controllerContext, Type type, bool useEmptyPrefix = false)
        {
            var metadataForType = ModelMetadataProviders.Current.GetMetadataForType(null, type);
            var modelBindingContext = new ModelBindingContext
            {
                ModelName = type.Name,
                ModelMetadata = metadataForType,
                ValueProvider = formCollection.ToValueProvider(),
                FallbackToEmptyPrefix = useEmptyPrefix
            };
            var binder = new DefaultModelBinder().BindModel(controllerContext, modelBindingContext);
            return binder;
        }
    }
}