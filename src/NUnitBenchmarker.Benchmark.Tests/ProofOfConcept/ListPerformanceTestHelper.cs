using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
	internal class ListPerformanceTestHelper<T>
	{
		private static Random random;

		public static IEnumerable<T> GenerateItemsToAdd(ListPerformanceTestConfiguration<T> conf)
		{
			random = new Random(0);
			for (int i = 0; i < conf.Size; i++)
			{
				yield return CreateNewItem();
			}
		}

		/// <summary>
		/// Creates the new item to use in performance tests.
		/// </summary>
		/// <returns>Created item</returns>
		public static T CreateNewItem()
		{
			if (typeof(T) == typeof(int))
			{
				return (T)(object)random.Next(int.MinValue, int.MaxValue);
			}

			return Activator.CreateInstance<T>();
		}


		public static IList<T> CreateListInstance<T>(ListPerformanceTestConfiguration<T> conf)
		{
			return conf.ListType.MakeGenericType(typeof(T)).CreateInstance() as IList<T>;
		}
	}
}
