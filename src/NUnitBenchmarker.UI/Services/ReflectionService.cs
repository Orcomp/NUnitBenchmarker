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
    using Models;

    public class ReflectionService : IReflectionService
    {
        private readonly ICacheStorage<string, IEnumerable<Type>> _assemblyTypes = new CacheStorage<string, IEnumerable<Type>>();

        public AssemblyEntry GetAssemblyEntry(string assemblyPath, bool defaultIsChecked)
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            string fullName = assembly.FullName.Replace(", ", "\n");

            var assemblyEntry = new AssemblyEntry
            {
                Path = assemblyPath,
                Name = Path.GetFileName(assemblyPath),
                Description = string.Format("{0}\nLoaded from: {1}", fullName, assemblyPath)
            };

            var namespaces = GetNamespaces(assemblyEntry, assemblyPath, defaultIsChecked);
            assemblyEntry.InitializeChildren(namespaces);

            return assemblyEntry;
        }

        public IEnumerable<NamespaceEntry> GetNamespaces(AssemblyEntry assemblyEntry, string assemblyPath, bool defaultIsChecked)
        {
            Argument.IsNotNullOrWhitespace("assemblyPath", assemblyPath);

            var types = GetTypesFromAssembly(assemblyPath);
            var namespaces = types.Select(x => new NamespaceEntry(assemblyEntry)
            {
                Path = assemblyPath,
                Name = x.GetNamespace(),
                Description = x.GetNamespace()
            }).Distinct().OrderBy(e => e.Name).ToList();

            foreach (var space in namespaces)
            {
                var namespaceTypes = GetTypes(space, assemblyPath, defaultIsChecked);
                space.InitializeChildren(namespaceTypes);
            }

            return namespaces;
        }

        public IEnumerable<TypeEntry> GetTypes(NamespaceEntry namespaceEntry, string assemblyPath, bool defaultIsChecked)
        {
            Argument.IsNotNullOrWhitespace("assemblyPath", assemblyPath);

            var types = GetTypesFromAssembly(assemblyPath);
            return types.Where(x => string.Equals(x.GetNamespace(), namespaceEntry.Name))
                .Select(x => new TypeEntry(namespaceEntry)
                {
                    Path = assemblyPath,
                    TypeFullName = x.FullName,
                    Name = x.GetFriendlyName(),
                    IsChecked = defaultIsChecked,
                    Description = string.Format("{0}\n{1}", x.GetNamespace(), x.GetFriendlyName())
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