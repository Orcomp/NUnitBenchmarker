using System;

namespace NUnitBenchmarker.Benchmark
{
	internal class ImplementationInfo : IEquatable<ImplementationInfo>
	{
		public string AssemblyFileName { get; set; }
		public string TypeName { get; set; }
		public Type Type { get; set; }
		public string AssemblyQualifiedName { get; set; }

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <returns>
		/// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
		/// </returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(ImplementationInfo other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return string.Equals(TypeName, other.TypeName) && string.Equals(AssemblyFileName, other.AssemblyFileName) && string.Equals(AssemblyQualifiedName, other.AssemblyQualifiedName);
		}

		/// <summary>
		/// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// true if the specified object  is equal to the current object; otherwise, false.
		/// </returns>
		/// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
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
			if (obj.GetType() != this.GetType())
			{
				return false;
			}
			return Equals((ImplementationInfo) obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type. 
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"/>.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (TypeName != null ? TypeName.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (AssemblyFileName != null ? AssemblyFileName.GetHashCode() : 0);
				hashCode = (hashCode*397) ^ (AssemblyQualifiedName != null ? AssemblyQualifiedName.GetHashCode() : 0);
				return hashCode;
			}
		}

		public static bool operator ==(ImplementationInfo left, ImplementationInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ImplementationInfo left, ImplementationInfo right)
		{
			return !Equals(left, right);
		}
	}
}