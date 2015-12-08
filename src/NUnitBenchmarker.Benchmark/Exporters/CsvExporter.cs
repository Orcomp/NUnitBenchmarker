// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvExporter.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Exporters
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Data;
    using Logging;

    public class CsvExporter : ExporterBase
    {
        #region Fields
        private static readonly ILog Log = LogManager.GetLogger(typeof (PdfExporter));
        #endregion

        public void Export(BenchmarkResult result, string folderPath = null)
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
    }
}