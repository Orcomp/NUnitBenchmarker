using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using MigraDoc.DocumentObjectModel.Internals;
using NUnitBenchmarker.Benchmark.Configuration;
using NUnitBenchmarker.UIClient;
using NUnitBenchmarker.UIService.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SimpleSpeedTester.Core;
using SimpleSpeedTester.Core.OutcomeFilters;

namespace NUnitBenchmarker.Benchmark
{
	public static class Benchmarker
	{
		static Benchmarker()
		{
			_results = new Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>>();
			_testCases = new HashSet<string>();
			_timestamp = DateTime.Now;
		}

		// Testname, TestGroup, Tuple<TestCase, ellapsedTime>
		private static Dictionary<string, Dictionary<string, List<KeyValuePair<string, double>>>> _results;

		private static HashSet<string> _testCases;

		private static DateTime _timestamp;

		private const int NumberOfIterations = 5;

		public static void Benchmark(this Action action, IPerformanceTestCaseConfiguration conf, string testName, string testCase)
		{
			string testGroup = conf.Identifier;
			// Check config file to see if TestName should be ingored or not. 
			// If it should be ignored then 

			_testCases.Add(testCase);

			var test = new TestGroup(testGroup);

			var result = test.PlanAndExecute(testName, action, NumberOfIterations, new ExcludeMinAndMaxTestOutcomeFilter());
			
			//Log.InfoFormat("[{0}] {1} - {2}: {3} ms", testGroup, testName, testCase, result.AverageExecutionTime);
			UI.Logger.Info("[{0}] {1} - {2}: {3} ms", testGroup, testName, testCase, result.AverageExecutionTime);

			Save(testGroup, testName, testCase, result.AverageExecutionTime);
			UI.UpdateResult(new BenchmarkResult {Key = testName, Values = _results[testName], TestCases = _testCases.ToArray(), IsLast = conf.IsLast});
		}

		public static void Benchmark(this Action action, IPerformanceTestCaseConfiguration conf, string testName, int testCase)
		{
			Benchmark(action, conf, testName, testCase.ToString());
		}

		private static void Save(string testGroup, string testName, string testCase, double ellapsedTime)
		{
			if (!_results.ContainsKey(testName))
			{
				_results.Add(testName, new Dictionary<string, List<KeyValuePair<string, double>>>());
			}

			if (!_results[testName].ContainsKey(testGroup))
			{
				_results[testName].Add(testGroup, new List<KeyValuePair<string, double>>());
			}

			_results[testName][testGroup].Add(new KeyValuePair<string, double>(testCase, ellapsedTime));

			// NOTE we could update the plot after each save. 
		}

		/// <summary>
		///  For now we are plotting the results to a pdf file but it would be nice to plot to WPF application so people can zoom in 
		/// and out etc...
		/// </summary>
		public static void PlotResults()
		{
			foreach (var result in _results)
			{
				var plotModel = CreatePlotModel(new BenchmarkResult { Key = result.Key, Values = result.Value, TestCases = _testCases.ToArray()});
				ExportResults(plotModel, result.Key);
			}
		}

		public static PlotModel CreatePlotModel(BenchmarkResult result, bool isLinear = true)
		{
			var plotModel = new PlotModel(result.Key);
			plotModel.LegendTitle = "Legend";
			plotModel.LegendOrientation = LegendOrientation.Vertical;
			plotModel.LegendPlacement = LegendPlacement.Inside;
			plotModel.LegendPosition = LegendPosition.TopLeft;
			plotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
			plotModel.LegendBorder = OxyColors.Black;

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
				var lineSeries = new LineSeries();
				lineSeries.Title = series.Key;
				lineSeries.MarkerType = MarkerType.Diamond;
				lineSeries.MarkerSize = 3;

				var categoryIndex = 0;

				foreach (var dataPoint in series.Value)
				{
					lineSeries.Points.Add(new DataPoint(int.Parse(dataPoint.Key), dataPoint.Value));
					categoryIndex++;
				}

				plotModel.Series.Add(lineSeries);
			}
			return plotModel;
		}

		private static void ExportResults(PlotModel plotModel, string testName)
		{
			var pdfExporter = new PdfExporter();
			pdfExporter.Height = 400;
			pdfExporter.Width = 600;

			var folderPath = @"./Plots/Plots-" + _timestamp.ToString("yy-MM-dd-HH-mm-ss") + "/";
			Directory.CreateDirectory(folderPath);
			pdfExporter.Export(plotModel, File.Create(folderPath + testName + ".pdf"));

			PrintResults(folderPath + "results.csv");
		}

		/// <summary>
		/// Use this function to plot Categories if the TestCases are strings (instead of int)
		/// This will plot a column chart.
		/// </summary>
		public static void PlotCategoryResults()
		{
			foreach (var result in _results)
			{
				var plotModel = CreateCategoryPlotModel(new BenchmarkResult { Key = result.Key, Values = result.Value, TestCases = _testCases.ToArray() });
				ExportResults(plotModel, result.Key);
				//var d =
				//	BenchmarkFinalTabularData.Create(new BenchmarkResult
				//	{
				//		Key = result.Key,
				//		Values = result.Value,
				//		TestCases = _testCases.ToArray()
				//	});
			}
			
		}

		public static PlotModel CreateCategoryPlotModel(BenchmarkResult result, bool isLinear = false)
		{
			var plotModel = new PlotModel(result.Key);
			plotModel.LegendTitle = "Legend";
			plotModel.LegendOrientation = LegendOrientation.Vertical;
			plotModel.LegendPlacement = LegendPlacement.Inside;
			plotModel.LegendPosition = LegendPosition.TopRight;
			plotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
			plotModel.LegendBorder = OxyColors.Black;

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

			foreach (var result in _results)
			{
				var testName = result.Key;
				var seriesName = string.Empty;

				var testCases = new List<string>();
				var testResults = new Dictionary<string, List<string>>();

				foreach (var series in result.Value)
				{
					seriesName = series.Key;
					testCases.Clear();
					testResults.Add(seriesName, new List<string>());

					foreach (var dataPoint in series.Value)
					{
						testCases.Add(dataPoint.Key.ToString());
						testResults[seriesName].Add(dataPoint.Value.ToString("F"));
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
	}

	public class BenchmarkFinalTabularData
	{
		const string DescriptionColumnName = "Description";
		
		public string Title { get; set; }
		public DataTable DataTable { get; set; }


		public BenchmarkFinalTabularData(BenchmarkResult result)
		{
			Title = result.Key;
			var table = DataTable = new DataTable(Title);



			table.Columns.Add(DescriptionColumnName, typeof(string));
			foreach (var dataPoint in result.Values.FirstOrDefault().Value)
			{
				table.Columns.Add(dataPoint.Key, typeof(double));	
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
	}
}