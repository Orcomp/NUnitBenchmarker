using System;

namespace NUnitBenchmarker.Benchmark.Configuration
{
	public interface IPerformanceTestCaseConfiguration
	{
		string Identifier { get; set; }
		Type TargetImplementationType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this configuration is the last instance
		/// within a particular TestCase. 
		/// </summary>
		/// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
		bool IsLast { get; set; }
	}
}