// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResultExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Collections.Generic;
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

            return columnNames;
        }
    }
}