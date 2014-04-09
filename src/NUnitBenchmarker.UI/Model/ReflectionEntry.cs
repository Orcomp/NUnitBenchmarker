#region using...

using System;
using System.Collections.Generic;
using System.Reflection;

#endregion

namespace NUnitBenchmarker.Model
{
    /// <summary>
    ///     Class ReflectionEntry. Represents the model data for a Type
    /// </summary>
    public abstract class ReflectionEntry
    {
        public string Path { get; set; }
        public string AssemblyFullName { get; set; }
        public Assembly Assembly { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool UseIcons { get; set; }
        public bool LeafEntry { get; set; }
        public ReflectionEntry Parent { get; set; }

        public abstract IEnumerable<ReflectionEntry> GetChildren();

        public static string NormalizeNamespace(Type type)
        {
            return type.Namespace ?? "-";
        }

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
    }
}