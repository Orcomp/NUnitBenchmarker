// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Caching;
    using Catel.Reflection;
    using NUnitBenchmarker.Models;

    public class ReflectionService : IReflectionService
    {
        private readonly ICacheStorage<string, IEnumerable<Type>> _assemblyTypes = new CacheStorage<string, IEnumerable<Type>>();

        public AssemblyEntry GetAssemblyEntry(string assemblyPath)
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            string fullName = assembly.FullName.Replace(", ", "\n");

            var namespaces = GetNamespaces(assemblyPath);
            var assemblyEntry = new AssemblyEntry(namespaces)
            {
                Path = assemblyPath,
                Name = Path.GetFileName(assemblyPath),
                Description = string.Format("{0}\nLoaded from: {1}", fullName, assemblyPath)
            };

            return assemblyEntry;
        }

        public IEnumerable<NamespaceEntry> GetNamespaces(string assemblyPath)
        {
            Argument.IsNotNullOrWhitespace("assemblyPath", assemblyPath);

            var types = GetTypesFromAssembly(assemblyPath);
            return types.Select(x => new NamespaceEntry(GetTypes(assemblyPath, x.GetNamespace()))
            {
                Path = assemblyPath,
                Name = x.GetNamespace(),
                Description = x.GetNamespace(),
                LeafEntry = false
            }).Distinct().OrderBy(e => e.Name).ToList();
        }

        public IEnumerable<TypeEntry> GetTypes(string assemblyPath, string namespaceName)
        {
            Argument.IsNotNullOrWhitespace("assemblyPath", assemblyPath);

            var types = GetTypesFromAssembly(assemblyPath);
            return types.Where(x => string.Equals(x.GetNamespace(), namespaceName))
                .Select(x => new TypeEntry
                {
                    Path = assemblyPath,
                    TypeFullName = x.FullName,
                    Name = x.GetFriendlyName(),
                    Description = string.Format("{0}\n{1}", x.GetNamespace(), x.GetFriendlyName()),
                    LeafEntry = true
                }).OrderBy(e => e.Name).ToList();
        }

        private IEnumerable<Type> GetTypesFromAssembly(string assemblyPath)
        {
            return _assemblyTypes.GetFromCacheOrFetch(assemblyPath, () =>
            {
                var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
                var allTypes = new List<Type>(assembly.GetAllTypesSafely());

                return allTypes;
            });
        }
    }
}