﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Alexr03.Common.Misc.Enums
{
    public static class EnumHelper
    {
        /// <summary>
        /// Given an enum value, if a <see cref="DescriptionAttribute"/> attribute has been defined on it, then return that.
        /// Otherwise return the enum name.
        /// </summary>
        /// <typeparam name="T">Enum type to look in</typeparam>
        /// <param name="value">Enum value</param>
        /// <returns>Description or name</returns>
        public static string ToDescription<T>(this T value) where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("This is not an enum", nameof(T));
            }

            var fieldName = Enum.GetName(typeof(T), value);
            if (fieldName == null)
            {
                return string.Empty;
            }

            var fieldInfo = typeof(T).GetField(fieldName,
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
            if (fieldInfo == null)
            {
                return string.Empty;
            }

            var descriptionAttribute =
                (DescriptionAttribute) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .FirstOrDefault();
            if (descriptionAttribute == null)
            {
                return fieldInfo.Name;
            }

            return descriptionAttribute.Description;
        }
    }
}