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
			// Issue in NUnit: This constructor is called _earlier_ than TestFixtureSetup....
			// so we can not call GetImplementations here, because FindImplementatins was not called yet :-(


			// Until this issue is not workarouded Benchmarker.FindImplementations will be called multiple times...
			Benchmarker.FindImplementations(typeof(IList<>), true);
			Implementations = Benchmarker.GetImplementations().ToList();

		}

		public IEnumerable<ListPerformanceTestCaseConfiguration<T>> TestCases()
		{
			// Issue in NUnit: even this method is called _earlier_ than TestFixtureSetup....
			// so we can not call GetImplementations here, because FindImplementatins was not called yet :-(

			//if (Implementations == null)
			//{
			//	Implementations = Benchmarker.GetImplementations().ToList();
			//}
			
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