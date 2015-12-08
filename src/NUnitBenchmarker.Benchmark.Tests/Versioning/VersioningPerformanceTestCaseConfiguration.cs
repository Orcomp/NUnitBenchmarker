// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningPerformanceTestCaseConfiguration.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.Versioning
{
    using NUnitBenchmarker.Configuration;

    public class VersioningPerformanceTestCaseConfiguration : PerformanceTestCaseConfigurationBase
    {
        public VersioningImplementation VersioningImplementation { get; set; }
    }
}