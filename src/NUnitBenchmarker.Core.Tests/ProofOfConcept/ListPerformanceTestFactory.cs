using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;
using NUnitBenchmarker.UIClient;

namespace NUnitBenchmarker.Core.Tests.ProofOfConcept
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
			var assemblyNames = UIService.GetAssemblyNames();
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