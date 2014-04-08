#region using...

using System;
using System.Collections.Generic;
using System.Linq;
using Fasterflect;
using NUnitBenchmarker.Benchmark.Helper;

#endregion

namespace NUnitBenchmarker.UI.Model
{
    public class NameSpaceEntry : ReflectionEntry, IEquatable<NameSpaceEntry>
    {
        #region IEquatable<NameSpaceEntry> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(NameSpaceEntry other)
        {
            return Name.Equals(other.Name);
        }

        #endregion

        public override IEnumerable<ReflectionEntry> GetChildren()
        {
            return Assembly.Types()
                //.Where(x => x.Implements(interfaceType))
                .Where(x => NormalizeNamespace(x) == Name)
                .Select(x => new TypeEntry
                {
                    Path = Path,
                    AssemblyFullName = AssemblyFullName,
                    Assembly = Assembly,
                    TypeFullName = x.FullName,

                    Name = x.GetFriendlyName(),
                    Description = string.Format("{0}\n{1}", x.Namespace, x.GetFriendlyName()),
                    LeafEntry = true
                }).OrderBy(e => e.Name).ToList();
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((NameSpaceEntry)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHash	CodeCallInGetHashCode
            return Name == null ? base.GetHashCode() : Name.GetHashCode();
        }

        public static bool operator ==(NameSpaceEntry left, NameSpaceEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NameSpaceEntry left, NameSpaceEntry right)
        {
            return !Equals(left, right);
        }
    }
}