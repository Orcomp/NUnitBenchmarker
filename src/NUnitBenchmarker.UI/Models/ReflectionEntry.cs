// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    ///     Class ReflectionEntry. Represents the model data for a Type
    /// </summary>
    public abstract class ReflectionEntry
    {
        #region Properties
        public string Path { get; set; }
        //public string AssemblyFullName { get; set; }
        //public Assembly Assembly { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool UseIcons { get; set; }
        public bool LeafEntry { get; set; }
        public ReflectionEntry Parent { get; set; }
        #endregion

        #region Methods
        public abstract IEnumerable<ReflectionEntry> GetChildren();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}