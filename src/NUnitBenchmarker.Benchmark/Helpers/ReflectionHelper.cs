// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NUnitBenchmarker
{
    using System;
    using System.Linq;

    public static class ReflectionHelper
    {
        #region Methods
        public static string GetFriendlyName(this Type type)
        {
            if (type == typeof (int))
            {
                return "int";
            }

            if (type == typeof (short))
            {
                return "short";
            }

            if (type == typeof (byte))
            {
                return "byte";
            }

            if (type == typeof (bool))
            {
                return "bool";
            }

            if (type == typeof (long))
            {
                return "long";
            }

            if (type == typeof (float))
            {
                return "float";
            }

            if (type == typeof (double))
            {
                return "double";
            }

            if (type == typeof (decimal))
            {
                return "decimal";
            }

            if (type == typeof (string))
            {
                return "string";
            }

            if (type.IsGenericType)
            {
                return type.Name.Split('`')[0] + "<" + string.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)) + ">";
            }

            return type.Name;
        }
        #endregion
    }
}