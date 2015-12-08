// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningPerformanceTest.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.Versioning
{
    using NUnit.Framework;

    [TestFixture]
    public class VersioningPerformanceTest
    {
        [Test, TestCaseSource(typeof (VersioningPerformanceTestFactory), "TestCases")]
        public void MagicForEachVersion(VersioningPerformanceTestCaseConfiguration configuration)
        {
            configuration.TestName = "DoMagic";

            configuration.Prepare = (i =>
            {
                var c = (VersioningPerformanceTestCaseConfiguration)i;
                c.IsReusable = true;
            });

            configuration.Run = (i =>
            {
                var c = (VersioningPerformanceTestCaseConfiguration)i;
                c.VersioningImplementation.DoMagic();
            });

            configuration.Benchmark(configuration.TestName, configuration.ToString(), 5);
        }
    }
}