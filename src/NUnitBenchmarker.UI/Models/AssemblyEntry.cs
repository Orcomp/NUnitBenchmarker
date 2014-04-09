// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using Catel;
    using System.Collections.Generic;

    public class AssemblyEntry : ReflectionEntry
    {
        private readonly List<ReflectionEntry> _children; 

        public AssemblyEntry(IEnumerable<ReflectionEntry> children)
        {
            Argument.IsNotNull("children", children);

            _children = new List<ReflectionEntry>(children);
        }

        #region Methods
        public override IEnumerable<ReflectionEntry> GetChildren()
        {
            return _children;
        }
        #endregion
    }
}