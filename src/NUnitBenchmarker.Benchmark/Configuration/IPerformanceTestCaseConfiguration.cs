// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPerformanceTestCaseConfiguration.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Configuration
{
    using System;

    public interface IPerformanceTestCaseConfiguration
    {
        #region Properties
        string Identifier { get; set; }
        Type TargetImplementationType { get; set; }
        #endregion
    }
}