using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using NUnitBenchmarker.UIClient;

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
	public class ListPerformanceTestFactory<T>
	{
		public IList<Type> Implementations { get; private set; }

		public ListPerformanceTestFactory()
		{
			Implementations = FindImplementations().ToList();
		}

		private IEnumerable<Type> FindImplementations()
		{
			// Legacy proof of concept:
			// return Assembly.Load("NUnitBenchmarker.Tests.Targets").Types().Where(x => x.Implements(typeof(IList<>))).Select(x => x).ToList();

			var result = new List<Type>();
			var assemblyNames = UI.GetAssemblyNames();
			foreach (var assemblyName in assemblyNames)
			{
				result.AddRange(Assembly.LoadFrom(assemblyName)
					.Types()
					.Where(x => x.Implements(typeof(IList<>)))
					.ToList());
			}
			return result;
		}

		public IEnumerable<ListPerformanceTestCaseConfiguration<T>> TestCases()
		{
			var lastImplementation = Implementations.LastOrDefault();

			foreach (var implementation in Implementations)
			{
				var identifier = string.Format("{0}", implementation.Name);

				yield return new ListPerformanceTestCaseConfiguration<T>()
				{
					Identifier = identifier,
					TargetImplementationType = implementation,
					IsLast = false,
					Size = 100,
					DummyForTesting = 0
				};

				yield return new ListPerformanceTestCaseConfiguration<T>()
				{
					Identifier = identifier,
					TargetImplementationType = implementation,
					IsLast = false,
					Size = 1000,
					DummyForTesting = 0
				};


				yield return new ListPerformanceTestCaseConfiguration<T>()
				{
					Identifier = identifier,
					TargetImplementationType = implementation,
					IsLast = false,
					Size = 10000,
					DummyForTesting = 0
				};

				yield return new ListPerformanceTestCaseConfiguration<T>()
				{
					Identifier = identifier,
					TargetImplementationType = implementation,
					IsLast = implementation == lastImplementation,
					Size = 100000,
					DummyForTesting = 0
				};
			}
		}
	}
}