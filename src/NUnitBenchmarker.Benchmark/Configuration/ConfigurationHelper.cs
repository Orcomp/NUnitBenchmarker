using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace NUnitBenchmarker.Benchmark.Configuration
{
	/// <summary>
	/// ConfigurationHelper is a simple static helper to ease loading NUnitBenchmarker configuration
	/// </summary>
	public static class ConfigurationHelper
	{
		/// <summary>
		/// Loads the specified configuration from the given file name.
		/// </summary>
		/// <param name="configFileName">Name of the configuration file. If not presented the standard
		/// .NET config file will be loaded</param>
		/// <returns>The loaded NUnitBenchmarkerConfigurationSection.</returns>
		public static NUnitBenchmarkerConfigurationSection Load(string configFileName = null)
		{
			const string sectionName = "NUnitBenchmarkerConfigSection";
			if (configFileName == null)
			{
				return Check((NUnitBenchmarkerConfigurationSection)ConfigurationManager.GetSection(sectionName));
			}
			var configMap = new ExeConfigurationFileMap
			{
				ExeConfigFilename = configFileName
			};

			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
			
			return Check((NUnitBenchmarkerConfigurationSection) config.GetSection(sectionName));
		}

		/// <summary>
		/// Checks the specified configuration for additional validation errors like invalid regex etc
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <returns>NUnitBenchmarkerConfigurationSection.</returns>
		private static NUnitBenchmarkerConfigurationSection Check(NUnitBenchmarkerConfigurationSection configuration)
		{
			//string exeFolder = ".";
			//try
			//{
			//	exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			//	exeFolder = exeFolder.Replace(@"file:\", "");
			//}
			//catch (Exception e)
			//{
				
			//}
			
			if (configuration != null)
			{
				System.Configuration.Configuration currentConfiguration = configuration.CurrentConfiguration;
				if (currentConfiguration == null)
				{
					configuration.ConfigFile = "";
				}
				else
				{
					configuration.ConfigFile = currentConfiguration.FilePath;	
				}
			}
			else
			{
				configuration = new NUnitBenchmarkerConfigurationSection();
				configuration.SearchFolders.Add(new SearchFolder {Folder = "."});
				//configuration.SearchFolders.Add(new SearchFolder {Folder = exeFolder});
			}

			//if (configuration.SearchFolders.Count == 0)
			//{
			//	var oldConfiguration = configuration;
			//	configuration = new NUnitBenchmarkerConfigurationSection();
			//	configuration.SearchFolders.Add(new SearchFolder { Folder = "." });
			//	//configuration.SearchFolders.Add(new SearchFolder { Folder = exeFolder });
			//	configuration.DisplayUI = oldConfiguration.DisplayUI;
			//	foreach (var item in oldConfiguration.ImplementationFilters)
			//	{
			//		configuration.ImplementationFilters.Add((ExcludeIncludeElement) item);
			//	}

			//	foreach (var item in oldConfiguration.TestCaseFilters)
			//	{
			//		configuration.TestCaseFilters.Add((ExcludeIncludeElement)item);
			//	}
			//}
			
			// TODO: Check for valid settings
			return configuration;
		}
	}
}
