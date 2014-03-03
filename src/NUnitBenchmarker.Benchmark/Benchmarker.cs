﻿#region using...

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Fasterflect;
using NUnitBenchmarker.Benchmark.Configuration;
using NUnitBenchmarker.UIClient;
using NUnitBenchmarker.UIService.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SimpleSpeedTester.Core;
using SimpleSpeedTester.Core.OutcomeFilters;

#endregion

namespace NUnitBenchmarker.Benchmark
{
	public static class Benchmarker
	{
		private const int NumberOfIterations = 5;
		private static readonly Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>> Results;
		private static readonly HashSet<string> TestCases;
		private static readonly NUnitBenchmarkerConfigurationSection Configuration;
		private static readonly bool HasConfiguration;
		
		private static DateTime _timestamp;
		private static Type _interfaceType;
		private static IEnumerable<ImplementationInfo> _implementationInfos;

		static Benchmarker()
		{
			Results = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>>();
			TestCases = new HashSet<string>();
			Configuration = ConfigurationHelper.Load();
			HasConfiguration = Configuration.ConfigFile != null;

			_timestamp = DateTime.Now;
		}

		private static void FindImplementations(Type interfaceType, bool displayUI = false)
		{
			// TODO: Implement locking for multithreading scenarios
			if (_implementationInfos != null && _interfaceType == interfaceType)
			{
				return;
			}

			// Override settings if there was no configuration file
			if (!HasConfiguration)
			{
				Configuration.DisplayUI = displayUI;
			}
			_interfaceType = interfaceType;
			UI.DisplayUI = Configuration.DisplayUI;

			// Refresh or get implementations:
			_implementationInfos = FindImplementations(interfaceType, Configuration.SearchFolders);
		}

		private static IEnumerable<ImplementationInfo> FindImplementations(Type interfaceType,
			SearchFolderCollection searchFolders)
		{
			var result = new List<ImplementationInfo>();
			foreach (SearchFolder searchFolder in searchFolders)
			{
				result.AddRange(FindImplementations(interfaceType, searchFolder));
			}
			return result;
		}

		private static IEnumerable<ImplementationInfo> FindImplementations(Type interfaceType, SearchFolder searchFolder)
		{
			var result = new List<ImplementationInfo>();
			var assemblyFileNames = GetAssemblyFileNames(searchFolder);
			foreach (var assemblyFileName in assemblyFileNames)
			{
				result.AddRange(FindImplementations(interfaceType, assemblyFileName));
			}

			return result;
		}

		private static IEnumerable<ImplementationInfo> FindImplementations(Type interfaceType, string assemblyFileName)
		{
			return Assembly.LoadFrom(assemblyFileName)
				.Types()
				.Where(x => x.Implements(interfaceType))
				.Select(x => new ImplementationInfo
				{
					AssemblyFileName = assemblyFileName,
					AssemblyQualifiedName = x.AssemblyQualifiedName,
					TypeName = x.FullName,
					Type = x
				})
				.ToList();
		}

		private static IEnumerable<string> GetAssemblyFileNames(SearchFolder searchFolder)
		{
			var result = new List<string>();
			if (searchFolder.Folder == null)
			{
				return result;
			}

			var di = new DirectoryInfo(Path.GetFullPath(searchFolder.Folder));
			var fileInfos = di.GetFiles("*.dll", SearchOption.TopDirectoryOnly);
			result.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));

			fileInfos = di.GetFiles("*.exe", SearchOption.TopDirectoryOnly);
			result.AddRange(fileInfos.Select(fileInfo => fileInfo.FullName));

			result = ApplyFilter(result, searchFolder);
			return result;
		}

		private static List<string> ApplyFilter(IList<string> source, SearchFolder searchFolder)
		{
			var removables = new List<string>();
			if (searchFolder.Include.Length > 0)
			{
				removables.AddRange(source.Where(name => !name.Contains(searchFolder.Include)));
			}

			if (searchFolder.Exclude.Length > 0)
			{
				removables.AddRange(source.Where(name => name.Contains(searchFolder.Exclude)));
			}

			return source.Where(name => !removables.Contains(name)).ToList();
		}


		public static void Benchmark(this Action action,
			IPerformanceTestCaseConfiguration conf,
			string testName,
			string testCase)
		{
			var testGroup = conf.Identifier;
			// TODO: Check config file to see if TestName should be ingored or not. 


			TestCases.Add(testCase);

			var test = new TestGroup(testGroup);

			var result = test.PlanAndExecute(testName, action, NumberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());
			UI.Logger.Info("[{0}] {1} - {2}: {3} ms", testGroup, testName, testCase, result.AverageExecutionTime);

			Save(testGroup, testName, testCase, result.AverageExecutionTime);
			UI.UpdateResult(new BenchmarkResult
			{
				Key = testName,
				Values = Results[testName],
				TestCases = TestCases.ToArray(),
				IsLast = conf.IsLast
			});
		}

		public static void Benchmark(this Action action, IPerformanceTestCaseConfiguration conf, string testName, int testCase)
		{
			Benchmark(action, conf, testName, testCase.ToString(CultureInfo.InvariantCulture));
		}

		private static void Save(string testGroup, string testName, string testCase, double ellapsedTime)
		{
			if (!Results.ContainsKey(testName))
			{
				Results.Add(testName, new Dictionary<string, List<KeyValuePair<string, double>>>());
			}

			if (!Results[testName].ContainsKey(testGroup))
			{
				Results[testName].Add(testGroup, new List<KeyValuePair<string, double>>());
			}

			Results[testName][testGroup].Add(new KeyValuePair<string, double>(testCase, ellapsedTime));
		}

		/// <summary>
		///     For now we are plotting the results to a pdf file but it would be nice to plot to WPF application so people can
		///     zoom in
		///     and out etc...
		/// </summary>
		public static void PlotResults()
		{
			foreach (var result in Results)
			{
				var plotModel =
					CreatePlotModel(new BenchmarkResult {Key = result.Key, Values = result.Value, TestCases = TestCases.ToArray()});
				ExportResults(plotModel, result.Key);
			}
		}

		public static PlotModel CreatePlotModel(BenchmarkResult result, bool isLinear = true)
		{
			var plotModel = new PlotModel(result.Key)
			{
				LegendTitle = "Legend",
				LegendOrientation = LegendOrientation.Vertical,
				LegendPlacement = LegendPlacement.Inside,
				LegendPosition = LegendPosition.TopLeft,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendBorder = OxyColors.Black
			};

			var xAxis = new LinearAxis(AxisPosition.Bottom);
			plotModel.Axes.Add(xAxis);

			Axis yAxis;
			if (isLinear)
			{
				yAxis = new LinearAxis(AxisPosition.Left, 0)
				{
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)",
				};
			}
			else
			{
				yAxis = new LogarithmicAxis(AxisPosition.Left, 0)
				{
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)",
					Minimum = 0.01,
					UseSuperExponentialFormat = true
				};
			}

			plotModel.Axes.Add(yAxis);

			foreach (var series in result.Values)
			{
				var lineSeries = new LineSeries
				{
					Title = series.Key,
					MarkerType = MarkerType.Diamond,
					MarkerSize = 3
				};

				foreach (var dataPoint in series.Value)
				{
					lineSeries.Points.Add(new DataPoint(int.Parse(dataPoint.Key), dataPoint.Value));
				}

				plotModel.Series.Add(lineSeries);
			}
			return plotModel;
		}

		private static void ExportResults(PlotModel plotModel, string testName)
		{
			var pdfExporter = new PdfExporter
			{
				Height = 400,
				Width = 600
			};

			var folderPath = @"./Plots/Plots-" + _timestamp.ToString("yy-MM-dd-HH-mm-ss") + "/";
			Directory.CreateDirectory(folderPath);
			pdfExporter.Export(plotModel, File.Create(folderPath + testName + ".pdf"));

			PrintResults(folderPath + "results.csv");
		}

		/// <summary>
		///     Use this function to plot Categories if the TestCases are strings (instead of int)
		///     This will plot a column chart.
		/// </summary>
		public static void PlotCategoryResults()
		{
			foreach (var result in Results)
			{
				var plotModel =
					CreateCategoryPlotModel(new BenchmarkResult
					{
						Key = result.Key,
						Values = result.Value,
						TestCases = TestCases.ToArray()
					});
				ExportResults(plotModel, result.Key);
			}
		}

		public static PlotModel CreateCategoryPlotModel(BenchmarkResult result, bool isLinear = false)
		{
			var plotModel = new PlotModel(result.Key)
			{
				LegendTitle = "Legend",
				LegendOrientation = LegendOrientation.Vertical,
				LegendPlacement = LegendPlacement.Inside,
				LegendPosition = LegendPosition.TopRight,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendBorder = OxyColors.Black
			};

			var dateAxis = new CategoryAxis(AxisPosition.Bottom, "Categories", result.TestCases);
			plotModel.Axes.Add(dateAxis);

			Axis valueAxis;
			if (isLinear)
			{
				valueAxis = new LinearAxis(AxisPosition.Left, 0)
				{
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)"
				};
			}
			else
			{
				valueAxis = new LogarithmicAxis(AxisPosition.Left, 0)
				{
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)",
					Minimum = 0.01,
					UseSuperExponentialFormat = true,
					StartPosition = 1,
					EndPosition = 0
				};
			}


			plotModel.Axes.Add(valueAxis);

			foreach (var series in result.Values)
			{
				var columnSeries = new ColumnSeries {Title = series.Key};

				var categoryIndex = 0;
				foreach (var dataPoint in series.Value)
				{
					columnSeries.Items.Add(new ColumnItem(dataPoint.Value, categoryIndex));
					categoryIndex++;
				}

				plotModel.Series.Add(columnSeries);
			}
			return plotModel;
		}

		public static void PrintResults(string filePath)
		{
			var sb = new StringBuilder();

			foreach (var result in Results)
			{
				var testName = result.Key;

				var testCases = new List<string>();
				var testResults = new Dictionary<string, List<string>>();

				foreach (var series in result.Value)
				{
					testCases.Clear();
					testResults.Add(series.Key, new List<string>());

					foreach (var dataPoint in series.Value)
					{
						testCases.Add(dataPoint.Key);
						testResults[series.Key].Add(dataPoint.Value.ToString("F"));
					}
				}

				sb.AppendLine(testName);
				sb.AppendLine();
				sb.AppendLine("," + string.Join(",", testCases));

				foreach (var series in testResults)
				{
					sb.AppendLine(series.Key + "," + string.Join(",", series.Value.ToString()));
				}

				sb.AppendLine();
			}

			File.WriteAllText(filePath, sb.ToString());
		}

		public static IEnumerable<Type> GetImplementations(Type interfaceType, bool displayUI = false)
		{
			FindImplementations(interfaceType, displayUI);
			var result = new List<Type>();
			var typeSpecifications = UI.GetImplementations(new TypeSpecification(_interfaceType));

			var assemblies = new Dictionary<string, Assembly>();
			foreach (var spec in typeSpecifications)
			{
				Assembly assembly;
				if (assemblies.ContainsKey(spec.AssemblyPath))
				{
					assembly = assemblies[spec.AssemblyPath];
				}
				else
				{
					assembly = Assembly.LoadFrom(spec.AssemblyPath);
					assemblies.Add(spec.AssemblyPath, assembly);
				}
				result.AddRange(assembly
					.Types()
					.Where(t => t.FullName.Equals(spec.FullName))
					.Where(x => x.Implements(_interfaceType))
					.ToList());
			}

			result.AddRange(_implementationInfos.Select(i => i.Type));
			result = RemoveDuplicates(result).ToList();
			return result;
		}

		private static IEnumerable<T> RemoveDuplicates<T>(IEnumerable<T> items)
		{
			var set = new HashSet<string>();
			var result = new List<T>();

			// About this string stuff: Strange but the duplicates have _different_ hashcode...
			// Workaround here is using their string representation:
			foreach (var item in items.Where(item => !set.Contains(item.ToString())))
			{
				set.Add(item.ToString());
				result.Add(item);
			}
			return result;
		}
	}

	internal class ImplementationInfo
	{
		public string AssemblyFileName { get; set; }
		public string TypeName { get; set; }
		public Type Type { get; set; }
		public string AssemblyQualifiedName { get; set; }
	}

	public class BenchmarkFinalTabularData
	{
		private const string DescriptionColumnName = "Description";

		public BenchmarkFinalTabularData(BenchmarkResult result)
		{
			Title = result.Key;
			var table = DataTable = new DataTable(Title);

			table.Columns.Add(DescriptionColumnName, typeof (string));
			foreach (var dataPoint in result.Values.FirstOrDefault().Value)
			{
				table.Columns.Add(dataPoint.Key, typeof (double));
			}

			foreach (var series in result.Values)
			{
				var row = table.NewRow();
				table.Rows.Add(row);
				row[DescriptionColumnName] = series.Key;

				foreach (var dataPoint in series.Value)
				{
					row[dataPoint.Key] = dataPoint.Value;
					//row[dataPoint.Key] = dataPoint.Value.ToString("F");
				}
			}
		}

		public string Title { get; set; }
		public DataTable DataTable { get; set; }
	}
}