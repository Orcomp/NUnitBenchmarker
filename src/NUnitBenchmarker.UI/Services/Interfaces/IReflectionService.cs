// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System.Collections.Generic;
    using NUnitBenchmarker.Models;

    public interface IReflectionService
    {
        AssemblyEntry GetAssemblyEntry(string assemblyPath);
        IEnumerable<TypeEntry> GetTypes(NamespaceEntry namespaceEntry, string assemblyPath);
        IEnumerable<NamespaceEntry> GetNamespaces(AssemblyEntry assemblyEntry, string assemblyPath);
    }
}