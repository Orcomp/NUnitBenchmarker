using System;

namespace NUnitBenchmarker.Benchmark.Configuration
{
	public interface IPerformanceTestCaseConfiguration
	{
		string Identifier { get; set; }
		Type TargetImplementationType { get; set; }
	}
}