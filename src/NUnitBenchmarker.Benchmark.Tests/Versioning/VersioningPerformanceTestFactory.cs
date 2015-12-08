// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VersioningPerformanceTestFactory.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.Versioning
{
    using System;
    using System.Collections.Generic;

    public static class VersioningPerformanceTestFactory
    {
        // Note: the higher the number, the longer it takes but the more accurate the numbers will be
        private const int NumberOfRuns = 100;

        private static readonly List<Type> _implementations = new List<Type>();

        static VersioningPerformanceTestFactory()
        {
            _implementations.Add(typeof(VersioningImplementation));
        }

        public static IEnumerable<VersioningPerformanceTestCaseConfiguration> TestCases()
        {
            // Note: This file would be included in every project targeting a different version. NUnitBenchmarker will
            // automatically understand the different versioning. Now we add the versioning manually. These numbers are
            // shuffled so we can test if the visualizer automatically sorts them correctly
            var versions = new [] { "1.2", "2.0", "1.0", "1.1",  };

            foreach (var implementation in _implementations)
            {
                for (int i = 0; i < versions.Length; i++)
                {
                    yield return new VersioningPerformanceTestCaseConfiguration
                    {
                        VersioningImplementation = Activator.CreateInstance(implementation) as VersioningImplementation,
                        Identifier = implementation.GetFriendlyName(),
                        Version = versions[i],
                        TargetImplementationType = implementation,
                        Count = NumberOfRuns
                    };
                }
            }
        }
    }
}