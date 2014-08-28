#region Copyright (c) 2008 - 2014 Orcomp development team.
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmarker.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace NUnitBenchmarker
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Text.RegularExpressions;
	using Catel.Logging;
	using Configuration;
	using Data;
	using Fasterflect;
	using OxyPlot;
	using OxyPlot.Axes;
	using OxyPlot.Series;
	using SimpleSpeedTester.Core;
	using SimpleSpeedTester.Core.OutcomeFilters;
	using SimpleSpeedTester.Interfaces;

    #endregion

	public static class Benchmarker
	{
		#region Constants
		private const int SignificantDigitCount = 3;
		private static readonly ILog Log = LogManager.GetCurrentClassLogger();
		private static readonly Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>> Results;
		private static readonly HashSet<string> TestCases;
		private static readonly NUnitBenchmarkerConfigurationSection Configuration;
		private static readonly bool HasConfiguration;

		private static DateTime _timestamp;
		private static Type _interfaceType;
		private static IEnumerable<ImplementationInfo> _implementationInfos;
		#endregion

		#region Constructors
		static Benchmarker()
		{
			Results = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>>();
			TestCases = new HashSet<string>();
			Configuration = ConfigurationHelper.Load();
			HasConfiguration = Configuration.ConfigFile != null;
			if (HasConfiguration)
			{
				UI.DisplayUI = Configuration.DisplayUI;
			}

			_timestamp = DateTime.Now;
		}
		#endregion

		// Ensure static constructor was executed. This may or may not needed when using UI static class.
		// TODO: Resolve this by more professional way. Benchmarker's constructor must run before than UI's constructor
		// but this can not be achieved by referencing Benchmarker class in UI's static constructor because of circular reference

		#region Methods
		public static void Init()
		{
			UI.Ping("");
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

			// Refresh or get implementations:
			var findImplementations = FindImplementations(interfaceType, Configuration.SearchFolders);
			_implementationInfos = ApplyImplementationFilter(findImplementations, Configuration.ImplementationFilters.Cast<ExcludeIncludeElement>());
		}

		private static IEnumerable<ImplementationInfo> ApplyImplementationFilter(IEnumerable<ImplementationInfo> infos, IEnumerable<ExcludeIncludeElement> filters)
		{
			var excludeIncludeElements = filters as IList<ExcludeIncludeElement> ?? filters.ToList();

			var removables = new List<ImplementationInfo>();
			var implementationInfos = infos as IList<ImplementationInfo> ?? infos.ToList();
			if (excludeIncludeElements.Any(f => f.Include.Length > 0))
			{
				//removables.AddRange(implementationInfos.Where(i => excludeIncludeElements.Where(f => f.Include.Length > 0).All(f => !i.TypeName.Contains(f.Include))));
				removables.AddRange(implementationInfos
					.Where(i => excludeIncludeElements.Where(f => f.Include.Length > 0)
						.All(f => !Regex.IsMatch(i.TypeName, f.Include))));
			}

			//removables.AddRange(implementationInfos.Where(i => excludeIncludeElements.Where(f => f.Exclude.Length > 0).Any(f => i.TypeName.Contains(f.Exclude))));
			removables.AddRange(implementationInfos
				.Where(i => excludeIncludeElements.Where(f => f.Exclude.Length > 0)
					.Any(f => Regex.IsMatch(i.TypeName, f.Exclude))));
			return implementationInfos.Where(i => !removables.Contains(i)).ToList();
		}

		private static IEnumerable<ImplementationInfo> FindImplementations(Type interfaceType, SearchFolderCollection searchFolders)
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
				.Where(x => x.Implements(interfaceType) && x.IsClass)
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

			result = ApplyNameFilter(result, searchFolder);
			return result;
		}

		private static List<string> ApplyNameFilter(IList<string> assemblyNames, SearchFolder searchFolder)
		{
			var removables = new List<string>();
			if (searchFolder.Include.Length > 0)
			{
				removables.AddRange(assemblyNames.Where(an => !an.Contains(searchFolder.Include)));
			}

			if (searchFolder.Exclude.Length > 0)
			{
				removables.AddRange(assemblyNames.Where(an => an.Contains(searchFolder.Exclude)));
			}

			return assemblyNames.Where(an => !removables.Contains(an)).ToList();
		}

		public static void Benchmark(this IPerformanceTestCaseConfiguration configuration, string testName, int testCase, int count = 0)
		{
			Benchmark(configuration, testName, testCase.ToString(CultureInfo.InvariantCulture), count);
		}

		public static void Benchmark(this IPerformanceTestCaseConfiguration configuration, string testName, string testCase, int count = 0)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}

			if (configuration.Run == null)
			{
				throw new ArgumentNullException("configuration.Run");
			}

			// There was explicit value given for the optional parameter
			if (count > 0)
			{
				configuration.Count = count;
			}

			// Initalize the Count in case the user forgot. We do not interpre 0 as 0 times execution
			if (configuration.Count == 0)
			{
				configuration.Count = 1;
			}

			if (configuration.Count > 1)
			{
				// TODO
			}

			var testGroup = configuration.Identifier;
			if (FilterTestCaseOut(testCase, Configuration.TestCaseFilters.Cast<ExcludeIncludeElement>()))
			{
				return;
			}

			TestCases.Add(testCase);

			var test = new TestGroup(testGroup);
            var averageExecutionTime = 0.0;
            ITestResultSummary result = null;

			if (configuration.Prepare != null && !configuration.IsReusable)
			{
				var sum = 0.0;
				var min = double.MaxValue;
				var max = double.MinValue;
				for (var i = 0; i < configuration.Count; i++)
				{
					configuration.Prepare(configuration);
				    result = test.PlanAndExecute(testName, configuration.Run, configuration, 1);

                    sum += result.AverageExecutionTime;
					if (result.AverageExecutionTime < min)
					{
						min = result.AverageExecutionTime;
					}
					if (result.AverageExecutionTime > max)
					{
						max = result.AverageExecutionTime;
					}
				}
				var correctedCount = configuration.Count;
				if (configuration.Count > 3)
				{
					sum -= (min + max);
					correctedCount -= 2;
				}
				averageExecutionTime = sum / correctedCount;
			}
			else
			{
				if (configuration.Prepare != null)
				{
					configuration.Prepare(configuration);
				}

				result = test.PlanAndExecute(testName, configuration.Run, configuration, configuration.Count, new ExcludeMinAndMaxTestOutcomeFilter());
				averageExecutionTime = result.AverageExecutionTime;
			}
            averageExecutionTime = (averageExecutionTime / configuration.Divider).RoundToSignificantDigits(SignificantDigitCount);

            result.TestResult.Outcomes.ForEach(outcome =>
            {
                if (outcome.Exception != null)
                {
                    Log.Info("[{0}] {1} - {2}: {3}", testGroup, testName, NumericUtils.TryToFormatAsNumber(testCase), outcome.Exception.GetType());
                }
            });
			Log.Info("[{0}] {1} - {2}: {3} (ms)", testGroup, testName, NumericUtils.TryToFormatAsNumber(testCase), averageExecutionTime);

			Save(testGroup, testName, testCase, averageExecutionTime);
			var benchmarkResult = new BenchmarkResult
			{
				TypeSpecification = new TypeSpecification(configuration.TargetImplementationType),
				Key = testName,
				Values = Results[testName],
				TestCases = TestCases.ToArray(),
			};

			UI.UpdateResult(benchmarkResult);
			var assert = configuration.Assert;
			if (assert != null)
			{
				configuration.Assert(configuration);
			}
		}

		private static bool FilterTestCaseOut(string testCase, IEnumerable<ExcludeIncludeElement> filters)
		{
			var excludeIncludeElements = filters as IList<ExcludeIncludeElement> ?? filters.ToList();

			if (excludeIncludeElements.Any(f => f.Include.Length > 0))
			{
				if (excludeIncludeElements.Where(f => f.Include.Length > 0).All(f => !Regex.IsMatch(testCase, f.Include)))
				{
					return true;
				}
			}

			if (excludeIncludeElements.Any(f => f.Exclude.Length > 0))
			{
				if (excludeIncludeElements.Where(f => f.Exclude.Length > 0).Any(f => Regex.IsMatch(testCase, f.Exclude)))
				{
					return true;
				}
			}

			return false;
		}

		//public static void Benchmark(this Action action, IPerformanceTestCaseConfiguration conf, string testName, int testCase)
		//{
		//	Benchmark(action, conf, testName, testCase.ToString(CultureInfo.InvariantCulture));
		//}

		private static void Save(string testGroup, string testName, string testCase, double elapsedTime)
		{
			//elapsedTime = RoundToSignificantDigits(elapsedTime, 3);

			if (!Results.ContainsKey(testName))
			{
				Results.Add(testName, new Dictionary<string, List<KeyValuePair<string, double>>>());
			}

			if (!Results[testName].ContainsKey(testGroup))
			{
				Results[testName].Add(testGroup, new List<KeyValuePair<string, double>>());
			}

			Results[testName][testGroup].Add(new KeyValuePair<string, double>(testCase, elapsedTime));
		}

		public static PlotModel CreatePlotModel(BenchmarkResult result, bool isLinear = true)
		{
			var plotModel = new PlotModel
			{
				Title = result.Key,
				LegendTitle = "Legend",
				LegendOrientation = LegendOrientation.Vertical,
				LegendPlacement = LegendPlacement.Inside,
				LegendPosition = LegendPosition.TopLeft,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendBorder = OxyColors.Black
			};

			var xAxis = new LinearAxis
			{
				Position = AxisPosition.Bottom
			};
			plotModel.Axes.Add(xAxis);

			Axis yAxis;
			if (isLinear)
			{
				yAxis = new LinearAxis
				{
					Position = AxisPosition.Left,
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)",
				};
			}
			else
			{
				yAxis = new LogarithmicAxis
				{
					Position = AxisPosition.Left,
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

		public static void ExportResultsToPdf(PlotModel plotModel, BenchmarkResult result, string folderPath = null)
		{
			var testName = result.Key;
			var pdfExporter = new PdfExporter
			{
				Height = 400,
				Width = 600
			};

			folderPath = GetFolderPath(folderPath);
			var fileName = Path.Combine(folderPath, testName) + ".pdf";
			pdfExporter.Export(plotModel, File.Create(fileName));
			Log.Info("PDF export for test {0} was successful to file '{1}'", testName, fileName);
		}

		private static string GetFolderPath(string folderPath)
		{
			if (folderPath == null)
			{
				folderPath = @"./Plots/Plots-" + _timestamp.ToString("yy-MM-dd-HH-mm-ss") + "/";
			}

			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			return folderPath;
		}

		public static void ExportAllResults()
		{
			foreach (var result in Results)
			{
				var benchmarkResult = new BenchmarkResult
				{
					Key = result.Key,
					Values = result.Value,
					TestCases = TestCases.ToArray()
				};

				int dummy;
				var plotModel = int.TryParse(benchmarkResult.TestCases.FirstOrDefault(), out dummy)
					? CreatePlotModel(benchmarkResult)
					: CreateCategoryPlotModel(benchmarkResult);

				ExportResultsToPdf(plotModel, benchmarkResult);
				ExportResultsToCsv(benchmarkResult);
			}
		}

		public static PlotModel CreateCategoryPlotModel(BenchmarkResult result, bool isLinear = false)
		{
			var plotModel = new PlotModel
			{
				Title = result.Key,
				LegendTitle = "Legend",
				LegendOrientation = LegendOrientation.Vertical,
				LegendPlacement = LegendPlacement.Inside,
				LegendPosition = LegendPosition.TopRight,
				LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
				LegendBorder = OxyColors.Black
			};

			//var dateAxis = new CategoryAxis(AxisPosition.Bottom, "Categories", result.TestCases)
			var dateAxis = new CategoryAxis
			{
				Position = AxisPosition.Bottom,
				Title = "Categories"
			};

			plotModel.Axes.Add(dateAxis);

			Axis valueAxis;
			if (isLinear)
			{
				valueAxis = new LinearAxis
				{
					Position = AxisPosition.Left,
					MajorGridlineStyle = LineStyle.Solid,
					MinorGridlineStyle = LineStyle.Dot,
					Title = "Time (ms)"
				};
			}
			else
			{
				valueAxis = new LogarithmicAxis
				{
					Position = AxisPosition.Left,
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

		public static void ExportResultsToCsv(BenchmarkResult result, string folderPath = null)
		{
			folderPath = GetFolderPath(folderPath);
			var sb = new StringBuilder();

			var testCases = new List<string>();
			var testResults = new Dictionary<string, List<string>>();

			foreach (var series in result.Values)
			{
				testCases.Clear();
				testResults.Add(series.Key, new List<string>());

				foreach (var dataPoint in series.Value)
				{
					testCases.Add(dataPoint.Key);
					testResults[series.Key].Add(dataPoint.Value.ToString(CultureInfo.CurrentCulture));
				}
			}

			sb.AppendLine("Description, " + string.Join(", ", testCases.Select(n => string.Format("{0} (ms)", NumericUtils.TryToFormatAsNumber(n)))));

			foreach (var series in testResults)
			{
				sb.AppendLine(series.Key + "," + string.Join(",", series.Value.Select(v => v.ToString())));
			}

			sb.AppendLine();

			var fileName = Path.Combine(folderPath, result.Key) + ".csv";
			File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);
			Log.Info("CSV export for test {0} was successful to file '{1}'", result.Key, fileName);
		}

		public static IEnumerable<Type> GetImplementations(Type interfaceType, bool displayUI = false)
		{
			FindImplementations(interfaceType, displayUI);
			var result = new List<Type>();
			var typeSpecification = new TypeSpecification(_interfaceType);
			var typeSpecifications = UI.GetImplementations(typeSpecification);

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
		#endregion
	}
}