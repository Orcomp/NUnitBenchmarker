// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkFinalTabularData.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using Data;

    public class BenchmarkFinalTabularData
    {
        #region Constants
        private const string DescriptionColumnName = "Description";
        #endregion

        #region Constructors
        public BenchmarkFinalTabularData(BenchmarkResult result)
        {
            Title = result.Key;
            var table = new DataTable(Title);

            table.Columns.Add(DescriptionColumnName, typeof(string));

            foreach (var value in result.Values)
            {
                foreach (var dataPoint in value.Value)
                {
                    var dataPointColumnName = GetColumnName(dataPoint.Key);
                    if (!table.Columns.Contains(dataPointColumnName))
                    {
                        var column = new DataColumn(dataPointColumnName, typeof(double));
                        column.Caption = GetColumnTitle(dataPoint.Key);

                        table.Columns.Add(column);
                    }
                }
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

            DataTable = table;
        }
        #endregion

        #region Properties
        public string Title { get; private set; }
        public DataTable DataTable { get; private set; }
        #endregion

        #region Methods
        private static string GetColumnTitle(string text)
        {
            return string.Format("{0} (ms)", NumericUtils.TryToFormatAsNumber(text));
        }

        private static string GetColumnName(string text)
        {
            return string.Format("a_{0}", text.Replace(".", "_").Replace(" ", "_"));
        }
        #endregion
    }
}