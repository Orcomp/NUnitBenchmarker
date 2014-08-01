#region Copyright (c) 2008 - 2014 Orcomp development team.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DoNoReflectClass.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace NUnitBenchmarker.Tests.Targets.DoNotReflect
{
	#region using...
	using System;

	#endregion

	public class DoNoReflectClass : IComparable, IEquatable<DoNoReflectClass>
	{
		#region IComparable Members
		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region IEquatable<DoNoReflectClass> Members
		public bool Equals(DoNoReflectClass other)
		{
			throw new NotImplementedException();
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
			return Equals((DoNoReflectClass) obj);
		}

		public override int GetHashCode()
		{
			throw new NotImplementedException();
		}
		#endregion

		public static bool operator ==(DoNoReflectClass left, DoNoReflectClass right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(DoNoReflectClass left, DoNoReflectClass right)
		{
			return !Equals(left, right);
		}
	}
}