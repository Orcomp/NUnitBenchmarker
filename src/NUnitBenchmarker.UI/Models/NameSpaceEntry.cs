// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NameSpaceEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Models
{
    using System;
    using System.Collections.Generic;

    public class NamespaceEntry : ReflectionEntry, IEquatable<NamespaceEntry>
    {
        #region Constructors
        public NamespaceEntry(IEnumerable<ReflectionEntry> children)
            : base(children)
        {
        }
        #endregion

        #region IEquatable<NameSpaceEntry> Members
        public bool Equals(NamespaceEntry other)
        {
            return Name.Equals(other.Name);
        }
        #endregion

        #region Methods
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

            return Equals((NamespaceEntry) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHash	CodeCallInGetHashCode
            return Name == null ? base.GetHashCode() : Name.GetHashCode();
        }
        #endregion

        public static bool operator ==(NamespaceEntry left, NamespaceEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NamespaceEntry left, NamespaceEntry right)
        {
            return !Equals(left, right);
        }
    }
}