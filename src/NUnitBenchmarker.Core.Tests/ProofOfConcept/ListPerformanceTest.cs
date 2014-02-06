using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using NUnitBenchmarker.Core.Benchmark;

namespace NUnitBenchmarker.Core.Tests.ProofOfConcept
{
	/// <summary>
	///		Class ListPerformanceTest. Please note: This is not a real unit test against Benchmarker.
	///		Instead this is a starter Proof of Concept which is much more like an integration test. 
	///		In this early phase of this project it serves to assemble the big picture together and 
	///		check what concepts work and what do not...
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
			// NOTE: There is nothing stopping us from showing up a User Interface here and allow the user to select the target assemblies.
			// as well as allow them to select which tests they want to run etc...
			// The key idea is that we can interact with the user at this point.
			// We can then pass the assemblies the user has selected to the TestFactory class below.

			// var test = new ListPerformanceTestFactory<int>();
		}

		[Test]
		public void Dummy()
		{
			
		}

		/// <summary>
		/// Plots the results.
		/// </summary>
		[TestFixtureTearDown]
		public void PlotResults()
		{
			// NOTE: I am plotting the results at the end, but there is nothing stopping us from updating the plots after each call to the  Benchmark() method.
			Benchmarker.PlotResults();
		}

		/// <summary>
		/// Tests the IList Add() method performance
		/// </summary>
		/// <param name="conf">The conf.</param>
		[Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
		[MaxTime(10000)]
		public void AddTest(ListPerformanceTestConfiguration<int> conf)
		{
			var action = new Action(() =>
			{
				foreach (var item in GenerateItemsToAdd(conf))
				{
					conf.List.Add(item);
				}
			});

			action.Benchmark(conf.List.GetType().Namespace, "Add", conf.ToString());
		}

		private IEnumerable<T> GenerateItemsToAdd<T>(ListPerformanceTestConfiguration<T> conf)
		{
			for (int i = 0; i < conf.Size; i++)
			{
				yield return conf.CreateNewItem();
			}
		}

	}
}
