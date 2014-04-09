// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using Catel;
    using Catel.Caching;
    using Catel.Logging;

    public static class ReflectionExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly ICacheStorage<Type, string> _namespaceCache = new CacheStorage<Type, string>(); 

        public static string GetNamespace(this Type type)
        {
            Argument.IsNotNull("type", type);

            return _namespaceCache.GetFromCacheOrFetch(type, () =>
            {
                try
                {
                    return type.Namespace ?? "-";
                }
                catch (Exception)
                {
                    Log.Error("Failed to retrieve namespace for type '{0}', falling back to '-'", type.FullName);

                    return "-";
                }
            });
        }
    }
}