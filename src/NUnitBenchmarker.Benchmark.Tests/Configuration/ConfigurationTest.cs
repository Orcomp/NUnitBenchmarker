using System;
using NUnit.Framework;
using NUnitBenchmarker.Benchmark.Configuration;

namespace NUnitBenchmarker.Benchmark.Tests.Configuration
{
	[TestFixture]
	public class ConfigurationTest
	{
		[Test]
		public void ImplementationFilterIncludeEmptyTest()
		{
			
		}


		[Test]
		public void LoadConfigurationFull()
		{
			var configuration = ConfigurationHelper.Load("TestConfigurations\\Full.config");
			Assert.IsTrue(configuration.DisplayUI);

			Assert.AreEqual(2, configuration.SearchFolders.Count);
			Assert.AreEqual("SFI1", configuration.SearchFolders[0].Include);
			Assert.AreEqual("SFE1", configuration.SearchFolders[0].Exclude);
			Assert.AreEqual("SF1", configuration.SearchFolders[0].Folder);
			Assert.AreEqual("SFI2", configuration.SearchFolders[1].Include);
			Assert.AreEqual("SFE2", configuration.SearchFolders[1].Exclude);
			Assert.AreEqual("SF2", configuration.SearchFolders[1].Folder);

			Assert.AreEqual(2, configuration.ImplementationFilters.Count);
			Assert.AreEqual("IFI1", configuration.ImplementationFilters[0].Include);
			Assert.AreEqual("IFE1", configuration.ImplementationFilters[0].Exclude);
			Assert.AreEqual("IFI2", configuration.ImplementationFilters[1].Include);
			Assert.AreEqual("IFE2", configuration.ImplementationFilters[1].Exclude);

			Assert.AreEqual(2, configuration.TestCaseFilters.Count);
			Assert.AreEqual("TFI1", configuration.TestCaseFilters[0].Include);
			Assert.AreEqual("TFE1", configuration.TestCaseFilters[0].Exclude);
			Assert.AreEqual("TFI2", configuration.TestCaseFilters[1].Include);
			Assert.AreEqual("TFE2", configuration.TestCaseFilters[1].Exclude);
		}

		[Test]
		public void LoadConfigurationEmpty()
		{
			var configuration = ConfigurationHelper.Load("TestConfigurations\\Empty.config");
			Assert.IsFalse(configuration.DisplayUI);

			Assert.AreEqual(1, configuration.SearchFolders.Count);
			Assert.AreEqual(0, configuration.ImplementationFilters.Count);
			Assert.AreEqual(0, configuration.TestCaseFilters.Count);
		}

		[Test]
		public void LoadConfigurationMissingSection()
		{
			var configuration = ConfigurationHelper.Load("TestConfigurations\\MissingSection.config");
			Assert.IsFalse(configuration.DisplayUI);

			Assert.AreEqual(1, configuration.SearchFolders.Count);
			Assert.AreEqual(0, configuration.ImplementationFilters.Count);
			Assert.AreEqual(0, configuration.TestCaseFilters.Count);
		}

		[Test]
		public void LoadConfigurationMissingDefinition()
		{
			var configuration = ConfigurationHelper.Load("TestConfigurations\\MissingDefinition.config");
			Assert.AreEqual(1, configuration.SearchFolders.Count);
			Assert.AreEqual(0, configuration.ImplementationFilters.Count);
			Assert.AreEqual(0, configuration.TestCaseFilters.Count);

		}




	}
}
