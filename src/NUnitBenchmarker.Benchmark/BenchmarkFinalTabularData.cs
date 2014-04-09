// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkFinalTabularData.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Data;
    using System.Linq;
    using Catel;
    using NUnitBenchmarker.Data;

    public class BenchmarkFinalTabularData
    {
        #region Constants
        private const string DescriptionColumnName = "Description";
        #endregion

        #region Constructors
        public BenchmarkFinalTabularData(BenchmarkResult result)
        {
            Argument.IsNotNull(() => result);

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
        #endregion

        #region Properties
        public string Title { get; set; }
        public DataTable DataTable { get; set; }
        #endregion

        #region Methods
        private static string GetColumnName(string text)
        {
            return string.Format("{0} (ms)", NumericUtils.TryToFormatAsNumber(text));
        }
        #endregion
    }
}