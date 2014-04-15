﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System.Collections.Generic;

    public class AssemblyEntry : ReflectionEntry
    {
        public AssemblyEntry(IEnumerable<ReflectionEntry> children)
            : base(children)
        {
            
        }
    }
}