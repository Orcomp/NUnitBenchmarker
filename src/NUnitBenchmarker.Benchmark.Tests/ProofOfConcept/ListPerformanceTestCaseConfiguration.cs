using System;
using System.Collections.Generic;
using NUnitBenchmarker.Benchmark.Configuration;

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
	/// <summary>
	/// Class ListPerformanceTestCaseConfiguration.
	/// TODO: Revise if generic T parameter is useful or overengineered
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListPerformanceTestCaseConfiguration<T> : PerformanceTestCaseConfigurationBase
	{
		private readonly Random random;
		
		public int Size { get; set; }
		public int DummyForTesting { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return string.Format("{0}", Size);
		}
	}
}