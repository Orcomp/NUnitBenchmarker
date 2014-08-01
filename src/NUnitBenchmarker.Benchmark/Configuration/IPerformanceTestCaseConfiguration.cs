#region Copyright (c) 2008 - 2014 Orcomp development team.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPerformanceTestCaseConfiguration.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace NUnitBenchmarker.Configuration
{
	#region using...
	using System;

	#endregion

	/// <summary>
	/// Interface for configuring, preparing and running performance tests
	/// </summary>
	public interface IPerformanceTestCaseConfiguration
	{
		#region Properties

		/// <summary>
		/// Gets or sets the Prepare action. The configuration parameter serves to communicate with the Run action
		/// </summary>
		/// <value>Prepare action with IPerformanceTestCaseConfiguration parameter</value>
		Action<IPerformanceTestCaseConfiguration> Prepare { get; set; }

		/// <summary>
		/// Gets or sets the Run action. The configuration parameter serves to communicate with the Prepare action
		/// </summary>
		/// <value>Run action with IPerformanceTestCaseConfiguration parameter</value>
		Action<IPerformanceTestCaseConfiguration> Run { get; set; }
		
		/// <summary>
		/// Gets or sets the count of how many times the test should run. 
		/// </summary>
		/// <value>How many times the test should run</value>
		int Count { get; set; }


		/// <summary>
		/// Gets or sets a value indicating whether this configuration instance is reusable within identical
		/// repeated tests
		/// </summary>
		/// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
		bool IsReusable { get; set; }
		
		/// <summary>
		/// Gets or sets the test case identifier, for display and report purposes
		/// </summary>
		/// <value>The identifier.</value>
		string Identifier { get; set; }
		
		/// <summary>
		/// Gets or sets the type of the target implementation type for the particular test case.
		/// Must be not null in the new versions
		/// </summary>
		/// <value>The type of the target implementation.</value>
		Type TargetImplementationType { get; set; }

		/// <summary>
		/// Divider is any positive number (by default is 1.0) what can be used to normalize 
		/// the benchmark results. NUnitBenchmarker divide the original result with the divider value. If a test configuration
		/// defines an execute action with multiple operations for some reason but want to display the benchmark value for one operation set this value to the number of operations.
		/// </summary>
		/// <value>The divider.</value>
		/// <exception cref="System.ArgumentException">Divider must be greater than zero.</exception>
		double Divider { get; set; }


		#endregion
	}
}