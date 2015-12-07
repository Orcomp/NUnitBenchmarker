// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using System.Collections.Generic;

    public static class ReflectionExtensions
    {
        private static readonly Dictionary<Type, string> _namespaceCache = new Dictionary<Type, string>(); 

        public static string GetNamespace(this Type type)
        {
            lock (_namespaceCache)
            {
                if (!_namespaceCache.ContainsKey(type))
                {
                    _namespaceCache[type] = GetNamespaceInternal(type);
                }

                return _namespaceCache[type];
            }
        }

        private static string GetNamespaceInternal(Type type)
        {
            try
            {
                return type.Namespace ?? "-";
            }
            catch (Exception)
            {
                //Log.Error("Failed to retrieve namespace for type '{0}', falling back to '-'", type.FullName);

                return "-";
            }
        }
    }
}