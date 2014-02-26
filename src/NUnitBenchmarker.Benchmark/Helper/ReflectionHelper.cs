#region using...

using System;
using System.Linq;

#endregion

namespace NUnitBenchmarker.Benchmark.Helper
{
	public static class ReflectionHelper
	{
		public static string GetFriendlyName(this Type type)
		{
			if (type == typeof (int))
			{
				return "int";
			}
			if (type == typeof (short))
			{
				return "short";
			}
			if (type == typeof (byte))
			{
				return "byte";
			}
			if (type == typeof (bool))
			{
				return "bool";
			}
			if (type == typeof (long))
			{
				return "long";
			}
			if (type == typeof (float))
			{
				return "float";
			}
			if (type == typeof (double))
			{
				return "double";
			}
			if (type == typeof (decimal))
			{
				return "decimal";
			}
			if (type == typeof (string))
			{
				return "string";
			}
			if (type.IsGenericType)
			{
				return type.Name.Split('`')[0] + "<" + String.Join(", ", type.GetGenericArguments().Select(GetFriendlyName)) + ">";
			}

			return type.Name;
		}
	}
}