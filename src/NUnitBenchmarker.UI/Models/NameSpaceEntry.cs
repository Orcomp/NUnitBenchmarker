// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameSpaceEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System;
    using System.Collections.Generic;

    public class NameSpaceEntry : ReflectionEntry, IEquatable<NameSpaceEntry>
    {
        private readonly List<ReflectionEntry> _children; 

        #region Constructors
        public NameSpaceEntry(IEnumerable<ReflectionEntry> children)
        {
            _children = new List<ReflectionEntry>(children);
        }
        #endregion

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

        #region Methods
        public override IEnumerable<ReflectionEntry> GetChildren()
        {
            return _children;
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

            return Equals((NameSpaceEntry) obj);
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
        #endregion

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