using System;

namespace NUnitBenchmarker.Benchmark.Configuration
{
	public abstract class PerformanceTestCaseConfigurationBase : IPerformanceTestCaseConfiguration
	{
		public string Identifier { get; set; }
		public Type TargetImplementationType { get; set; }
	}
}