using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnitBenchmarker.Benchmark.Helper;
using NUnitBenchmarker.UIClient;

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
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

			// NUnit issue: This will run _later_ than constructor and factory methods of TestCaseSource...
			Benchmarker.FindImplementations(typeof(IList<>), true);
		}

		[Test]
		public void WcfCommunicationProofofConcept()
		{
			// To use the UI (from any runner, or any host process) simply do the following:
			// 1) Reference the NUnitBenchmarker.UIClient assembly
			// 2) Use the static UIService class in it.
			// (make sure the UI is running, autostart optionally is coming later
			// for example:

			var response = UI.Ping("Hello from the runner.");
			if (UI.DisplayUI)
			{
				Assert.AreEqual("Welcome to the machine: Hello from the runner.", response);
			}
			else
			{
				Assert.AreEqual("Welcome to the loopback: Hello from the runner.", response);
			}
		}

		[Test]
		public void RemoteLoggingProofOfConcept()
		{
			UI.Logger.Info("Test log message");
		}



		static string GetTypeName(Type type)
		{
			var codeDomProvider = CodeDomProvider.CreateProvider("C#");
			var typeReferenceExpression = new CodeTypeReferenceExpression(new CodeTypeReference(type));
			using (var writer = new StringWriter())
			{
				codeDomProvider.GenerateCodeFromExpression(typeReferenceExpression, writer, new CodeGeneratorOptions());
				return writer.GetStringBuilder().ToString();
			}
		}

		 
		/// <summary>
		/// Plots the results.
		/// </summary>
		[TestFixtureTearDown]
		public void PlotResults()
		{
			// NOTE: I am plotting the results at the end, but there is nothing stopping us from updating the plots after each call to the  Benchmark() method.
			Benchmarker.PlotCategoryResults();
		}

		/// <summary>
		/// Tests the IList Add() method performance
		/// </summary>
		/// <param name="conf">The conf.</param>
		[Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
		[MaxTime(10000)]
		public void AddTest(ListPerformanceTestCaseConfiguration<int> conf)
		{
			var itemsToAdd = ListPerformanceTestHelper<int>.GenerateItemsToAdd(conf).ToArray();
			var target = ListPerformanceTestHelper<int>.CreateListInstance(conf);
			
			var action = new Action(() =>
			{
				foreach (var item in itemsToAdd)
				{
					target.Add(item);
				}
			});

			action.Benchmark(conf, "Add", conf.ToString());
		}

		/// <summary>
		/// Tests the IList Add() method performance
		/// </summary>
		/// <param name="conf">The conf.</param>
		[Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
		[MaxTime(10000)]
		public void RemoveTest(ListPerformanceTestCaseConfiguration<int> conf)
		{
			var itemsToAdd = ListPerformanceTestHelper<int>.GenerateItemsToAdd(conf).ToArray();
			var target = ListPerformanceTestHelper<int>.CreateListInstance(conf);
			foreach (var item in itemsToAdd)
			{
				target.Add(item);
			}

			var action = new Action(() =>
			{
				foreach (var item in itemsToAdd)
				{
					target.Remove(item);
				}
			});

			action.Benchmark(conf, "Remove", conf.ToString());
		}
	}
}
