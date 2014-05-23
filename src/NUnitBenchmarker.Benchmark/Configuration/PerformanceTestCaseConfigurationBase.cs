#region Copyright (c) 2008 - 2014 Orcomp development team.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PerformanceTestCaseConfigurationBase.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace NUnitBenchmarker.Configuration
{
	#region using...
	using System;

	#endregion

	public abstract class PerformanceTestCaseConfigurationBase : IPerformanceTestCaseConfiguration
	{
		#region IPerformanceTestCaseConfiguration Members

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
		/// Gets or sets the type of the target implementation type for the particular test case.
		/// Must be not null.
		/// </summary>
		/// <value>The type of the target implementation.</value>
		public Type TargetImplementationType { get; set; }
		#endregion
	}
}