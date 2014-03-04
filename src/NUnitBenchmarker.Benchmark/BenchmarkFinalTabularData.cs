using System.Data;
using System.Linq;
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