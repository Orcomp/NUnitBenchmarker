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
    /// TODO: Revise if generic T parameter is useful or overengineered
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListPerformanceTestCaseConfiguration<T> : PerformanceTestCaseConfigurationBase
    {
        #region Properties
        public int Size { get; set; }
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