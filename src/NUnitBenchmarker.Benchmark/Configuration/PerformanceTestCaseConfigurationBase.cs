// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceTestCaseConfigurationBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Configuration
{
    using System;
    using System.Dynamic;

    public abstract class PerformanceTestCaseConfigurationBase : IPerformanceTestCaseConfiguration
    {
        #region Fields
        private double _divider = 1.0;
        #endregion

        #region Methods
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Version))
            {
                return Version;
            }

            return TestName;
        }
        #endregion

        #region IPerformanceTestCaseConfiguration Members
        protected PerformanceTestCaseConfigurationBase()
        {
            Parameters = new ExpandoObject();
        }

        /// <summary>
        /// Gets or sets the Prepare action. The configuration parameter serves to communicate with the Run action
        /// </summary>
        /// <value>Prepare action with IPerformanceTestCaseConfiguration parameter</value>
        public Action<IPerformanceTestCaseConfiguration> Prepare { get; set; }

        /// <summary>
        /// Gets or sets the Run action. The configuration parameter serves to communicate with the Prepare action
        /// </summary>
        /// <value>Run action with IPerformanceTestCaseConfiguration parameter</value>
        public Action<IPerformanceTestCaseConfiguration> Run { get; set; }

        /// <summary>
        /// Gets or sets the optional Assert action. The configuration parameter serves to communicate with the Run action
        /// This action will be called once after the multiple calls of the Run action
        /// </summary>
        /// <value>Assert action with IPerformanceTestCaseConfiguration parameter</value>
        public Action<IPerformanceTestCaseConfiguration> Assert { get; set; }

        /// <summary>
        /// Gets or sets the count of how many times the test should run. 
        /// </summary>
        /// <value>How many times the test should run</value>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this configuration instance is reusable within identical
        /// repeated tests
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        public bool IsReusable { get; set; }

        /// <summary>
        /// Gets or sets the test case identifier, for display and report purposes
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the target implementation type for the particular test case.
        /// Must be not null.
        /// </summary>
        /// <value>The type of the target implementation.</value>
        public Type TargetImplementationType { get; set; }

        /// <summary>
        /// Divider is any positive number (by default is 1.0) what can be used to normalize 
        /// the benchmark results. NUnitBenchmarker divide the original result with the divider value. If a test configuration
        /// defines an execute action with multiple operations for some reason but want to display the benchmark value for one operation set this value to the number of operations.
        /// </summary>
        /// <value>The divider.</value>
        /// <exception cref="System.ArgumentException">Divider must be greater than zero.</exception>
        public double Divider
        {
            get { return _divider; }
            set
            {
                if (value <= 0.0)
                {
                    throw new ArgumentException("Divider must be greater than zero.");
                }
                _divider = value;
            }
        }

        public int Size { get; set; }
        public string TestName { get; set; }
        public dynamic Parameters { get; private set; }
        #endregion
    }
}