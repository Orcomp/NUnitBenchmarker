// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListPerformanceTestCaseConfiguration.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
    using System.Collections.Generic;
    using NUnitBenchmarker.Configuration;

    /// <summary>
    /// Class ListPerformanceTestCaseConfiguration.
    /// </summary>
    public class ListPerformanceTestCaseConfiguration : PerformanceTestCaseConfigurationBase
    {
        #region Properties
        public int DummyForTesting { get; set; }
        public int[] Items { get; set; }
        public IList<int> Target { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}", Size);
        }
        #endregion
    }
}