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

		public IEnumerable<ListPerformanceTestConfiguration<T>> TestCases()
		{
			//var result = new List<ListPerformanceTestConfiguration<T>>();
			foreach (var implementation in Implementations)
			{
				//var identifier = string.Format("{0} : {1}", implementation.Namespace, implementation.Name);
				var identifier = string.Format("{0}", implementation.Name);

				yield return new ListPerformanceTestConfiguration<T>(identifier, implementation, 100, 0);
				yield return new ListPerformanceTestConfiguration<T>(identifier, implementation, 1000, 0);
				yield return new ListPerformanceTestConfiguration<T>(identifier, implementation, 10000, 0);
				yield return new ListPerformanceTestConfiguration<T>(identifier, implementation, 100000, 0);
				
				//result.Add(new ListPerformanceTestConfiguration<T>(identifier, implementation, 1000, 0));
				//result.Add(new ListPerformanceTestConfiguration<T>(identifier, implementation, 10000, 0));
				//result.Add(new ListPerformanceTestConfiguration<T>(identifier, implementation, 100000, 0));
				//result.Add(new ListPerformanceTestConfiguration<T>(identifier, implementation, 1000000, 0));
			}
			//return result;
		}
	}
}