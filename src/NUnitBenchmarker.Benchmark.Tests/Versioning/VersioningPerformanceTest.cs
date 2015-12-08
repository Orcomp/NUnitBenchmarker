// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningPerformanceTest.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.Versioning
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VersioningPerformanceTest
    {
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            var resultsFolder = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "results");
            resultsFolder = Path.GetFullPath(resultsFolder);

            // When using the UI, we will export all tests instead of the local run only
            Benchmarker.ExportAllResultsInUi(resultsFolder);
        }

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