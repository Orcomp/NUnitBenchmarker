// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceTestCaseConfigurationBase.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Configuration
{
    using System;

    public abstract class PerformanceTestCaseConfigurationBase : IPerformanceTestCaseConfiguration
    {
        #region IPerformanceTestCaseConfiguration Members
        public string Identifier { get; set; }
        public Type TargetImplementationType { get; set; }
        #endregion
    }
}