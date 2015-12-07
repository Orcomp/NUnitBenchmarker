// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListPerformanceTestFactory.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ListPerformanceTestFactory
    {
        #region Constructors
        public ListPerformanceTestFactory()
        {
            // Issue in NUnit: This constructor is called _earlier_ than TestFixtureSetup....

            // It would be ideal if this class would be a Singleton. However this class instantiated
            // by the runner by calling its public parameterless constructor so we have no way to prevent
            // multiple instances exist. This will not cause any error, just an optimizaion thing.
            Implementations = Benchmarker.GetImplementations(typeof (IList<>), true).ToList();
        }
        #endregion

        #region Properties
        public IList<Type> Implementations { get; private set; }
        #endregion

        #region Methods
        public IEnumerable<ListPerformanceTestCaseConfiguration> TestCases()
        {
            // Issue in NUnit: even this method is called _earlier_ than TestFixtureSetup....
            // so we can not call GetImplementations here, because FindImplementatins was not called yet :-(

            var testBatches = new[] {100, 1000, 10000, 100000};

            foreach (var implementation in Implementations)
            {
                var identifier = string.Format("{0}", implementation.GetFriendlyName());

                for (int i = 0; i < testBatches.Length; i++)
                {
                    yield return new ListPerformanceTestCaseConfiguration
                    {
                        Identifier = identifier,
                        TargetImplementationType = implementation,
                        Size = testBatches[i],
                        DummyForTesting = 0
                    };
                }
            }
        }
        #endregion
    }
}