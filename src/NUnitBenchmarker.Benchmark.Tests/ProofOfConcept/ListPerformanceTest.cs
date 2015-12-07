#region Copyright (c) 2008 - 2014 Orcomp development team.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListPerformanceTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
    #region using...
    using System.Linq;
    using NUnit.Framework;

    #endregion

    /// <summary>
    /// 		Class ListPerformanceTest. Please note: This is not a real unit test against Benchmarker.
    /// 		Instead this is a starter Proof of Concept which is much more like an integration test.
    /// 		In this early phase of this project it serves to assemble the big picture together and
    /// 		check what concepts work and what do not...
    /// </summary>
    [TestFixture]
    public class ListPerformanceTest
    {
        /// <summary>
        /// Setups this instance.
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            // NUnit issue: This will run _later_ than constructor and factory methods of TestCaseSource...
            // ...so we are late here. Commented out.
            // Benchmarker.FindImplementations(typeof(IList<>), true);

            // Workaround: 

            // The end user will call Benchmarker.GetImplementations() to get implementations in the TestCaseSource 
            // constructor. GetImplementations will message to UI to get the must updated UI selected/deselcted
            // implementations. However GetImplementations will check if Benchmarker has internally filled discovered
            // implementations by configuration or by convention. If it is not filled then calls FindImplementations
            // and fills and caches this implementation list.  GetImplementation() returns always 
            // A + B where:
            // A: the lazy discovered (once) (and cached) implementations by configuration or by convention
            // B: the actual response from UI
            // Note: Both A and B can be empty.
        }

        //[Test]
        //public void RemoteLoggingProofOfConcept()
        //{
        //    UI.Logger.Info("Test log message");
        //}

        /// <summary>
        /// Exports the results to .pdf and .csv
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Benchmarker.ExportAllResults();
        }

        /// <summary>
        /// Tests the IList Add() method performance
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        [Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
        //[MaxTime(10000)]
        public void AddTest(ListPerformanceTestCaseConfiguration<int> configuration)
        {
            configuration.Prepare = (i =>
            {
                var c = (ListPerformanceTestCaseConfiguration<int>)i;
                c.Items = ListPerformanceTestHelper<int>.GenerateItemsToAdd(configuration).ToArray();
                c.Target = ListPerformanceTestHelper<int>.CreateListInstance(configuration);
                c.IsReusable = true;
            });

            configuration.Run = (i =>
            {
                var c = (ListPerformanceTestCaseConfiguration<int>)i;
                foreach (var item in c.Items)
                {
                    c.Target.Add(item);
                }
            });

            configuration.Benchmark("Add", configuration.ToString(), 5);
        }

        /// <summary>
        /// Tests the IList Add() method performance
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        [Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
        [MaxTime(10000)]
        public void RemoveTest(ListPerformanceTestCaseConfiguration<int> configuration)
        {
            configuration.Prepare = (i =>
            {
                var c = (ListPerformanceTestCaseConfiguration<int>)i;
                c.Items = ListPerformanceTestHelper<int>.GenerateItemsToAdd(configuration).ToArray();
                c.Target = ListPerformanceTestHelper<int>.CreateListInstance(configuration);
                c.IsReusable = false; // This is the default
            });

            configuration.Run = (i =>
            {
                var c = (ListPerformanceTestCaseConfiguration<int>)i;
                foreach (var item in c.Items)
                {
                    c.Target.Remove(item);
                }
            });

            configuration.Benchmark("Remove", configuration.ToString(), 10);
        }

        [Test]
        public void WcfCommunicationProofofConcept()
        {
            // To use the UI (from any runner, or any host process) simply do the following:
            // 1) Reference the NUnitBenchmarker.UIClient assembly
            // 2) Use the static UIService class in it.
            // (make sure the UI is running, autostart optionally is coming later
            // for example:

            var response = UI.Ping("Hello from the runner.");
            Assert.AreEqual(UI.DisplayUI ? "Welcome to the machine: Hello from the runner." : "Welcome to the loopback: Hello from the runner.", response);
        }
    }
}