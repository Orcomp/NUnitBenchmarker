// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResultExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;

    public static class BenchmarkResultExtensions
    {
        public static List<string> GetColumnNames(this BenchmarkResult benchmarkResult)
        {
            var columnNames = new List<string>();

            foreach (var value in benchmarkResult.Values)
            {
                foreach (var dataPoint in value.Value)
                {
                    if (!columnNames.Contains(dataPoint.Key))
                    {
                        columnNames.Add(dataPoint.Key);
                    }
                }
            }

            return columnNames.OrderBy(x => x).ToList();
        }

        public static Dictionary<string, List<KeyValuePair<string, double>>> GetTestResultRows(this BenchmarkResult benchmarkResult)
        {
            var results = new Dictionary<string, List<KeyValuePair<string, double>>>();

            foreach (var value in benchmarkResult.Values.OrderBy(x => x.Key))
            {
                if (!results.ContainsKey(value.Key))
                {
                    results.Add(value.Key, new List<KeyValuePair<string, double>>());
                }

                foreach (var dataPoint in value.Value.OrderBy(x => x.Key))
                {
                    results[value.Key].Add(new KeyValuePair<string, double>(dataPoint.Key, dataPoint.Value));
                }
            }

            var finalResults = new Dictionary<string, List<KeyValuePair<string, double>>>();
            foreach (var result in results.OrderBy(x => x.Key))
            {
                finalResults.Add(result.Key, result.Value);
            }

            return finalResults;
        }
    }
}