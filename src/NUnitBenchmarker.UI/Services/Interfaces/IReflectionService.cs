// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReflectionService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IReflectionService
    {
        AssemblyEntry GetAssemblyEntry(string assemblyPath, bool defaultIsChecked);
        IEnumerable<TypeEntry> GetTypes(NamespaceEntry namespaceEntry, string assemblyPath, bool defaultIsChecked);
        IEnumerable<NamespaceEntry> GetNamespaces(AssemblyEntry assemblyEntry, string assemblyPath, bool defaultIsChecked);
    }
}