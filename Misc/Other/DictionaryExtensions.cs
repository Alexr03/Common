using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Alexr03.Common.Misc.Other
{
    public static class DictionaryExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object @object)
        {
            var dictionary = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            if (@object == null) return dictionary;
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(@object))
                dictionary.Add(property.Name.Replace("_", "-"), property.GetValue(@object));
            return dictionary;
        }

        public static void MergeWith(this IDictionary<string, object> instance, IDictionary<string, object> from, bool combineValuesOfExisting = true)
        {
            foreach (var keyValuePair in from)
            {
                if (!instance.ContainsKey(keyValuePair.Key))
                {
                    instance[keyValuePair.Key] = keyValuePair.Value;
                }
                else
                {
                    if (combineValuesOfExisting)
                    {
                        var o = instance[keyValuePair.Key];
                        if (o is string f)
                        {
                            instance[keyValuePair.Key] = $"{f} {keyValuePair.Value}";
                        }
                    }
                }
            }
        }
    }
}