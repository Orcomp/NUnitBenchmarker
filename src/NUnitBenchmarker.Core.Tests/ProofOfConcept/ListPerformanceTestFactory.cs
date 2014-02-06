using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace NUnitBenchmarker.Core.Tests.ProofOfConcept
{
	public class ListPerformanceTestFactory<T>
	{
		public List<Type> Implementations { get; private set; }

		public ListPerformanceTestFactory()
		{
			Implementations = FindImplementations();
		}

		private List<Type> FindImplementations()
		{
			// TODO: Instead of Assembly.Load we could look at the current directory and LoadFrom() all the assemblies we find in it.
			try
			{
				return Assembly.Load("NUnitBenchmarker.Tests.Targets").Types().Where(x => x.Implements(typeof(IList<>))).Select(x => x).ToList();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}

		public IEnumerable<ListPerformanceTestConfiguration<T>> TestCases()
		{
			foreach (var implementation in Implementations)
			{
				// Instantiate the target implementation
				var list = implementation.MakeGenericType(typeof(T)).CreateInstance() as IList<T>;

				//yield return new ListPerformanceTestConfiguration<T>(list, 2);
				yield return new ListPerformanceTestConfiguration<T>(list, 1000000);
			}
		}
	}
}