using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnitBenchmarker.Core;
using NUnitBenchmarker.UIService.Data;

namespace NUnitBenchmarker.Benchmark
{
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
				var columnName = GetColumnName(dataPoint.Key);
				table.Columns.Add(columnName, typeof (double));
			}

			foreach (var series in result.Values)
			{
				var row = table.NewRow();
				table.Rows.Add(row);
				row[DescriptionColumnName] = series.Key;

				foreach (var dataPoint in series.Value)
				{
					var columnName = GetColumnName(dataPoint.Key);
					row[columnName] = dataPoint.Value;
				}
			}
		}

		private static string GetColumnName(string text)
		{
			return string.Format("{0} (ms)", NumericUtils.TryToFormatAsNumber(text));
		}

		public string Title { get; set; }
		public DataTable DataTable { get; set; }
	}
}