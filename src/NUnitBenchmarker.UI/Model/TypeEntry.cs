// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Model
{
    using System.Collections.Generic;

    public class TypeEntry : ReflectionEntry
    {
        #region Properties
        public string TypeFullName { get; set; }
        #endregion

        #region Methods
        public override IEnumerable<ReflectionEntry> GetChildren()
        {
            return new List<ReflectionEntry>();
        }
        #endregion
    }
}