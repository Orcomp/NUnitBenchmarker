// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResultExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Collections.Generic;
    using Data;

    public static class BenchmarkResultExtensions
    {
        public static void UpdateResults(this BenchmarkResult benchmarkResult, BenchmarkResult newResult)
        {
            benchmarkResult.Key = newResult.Key;
            benchmarkResult.TestCases = newResult.TestCases;

            foreach (var result in newResult.Values)
            {
                var groupedValues = new List<KeyValuePair<string, double>>();
                var existingValues = new HashSet<string>();

                // 1) Create new values
                foreach (var value in result.Value)
                {
                    existingValues.Add(value.Key);
                    groupedValues.Add(new KeyValuePair<string, double>(value.Key, value.Value));
                }

                // 2) Copy existing values
                if (benchmarkResult.Values.ContainsKey(result.Key))
                {
                    var existingGroupedValues = benchmarkResult.Values[result.Key];
                    foreach (var value in existingGroupedValues)
                    {
                        if (!existingValues.Contains(value.Key))
                        {
                            existingValues.Add(value.Key);
                            groupedValues.Add(new KeyValuePair<string, double>(value.Key, value.Value));
                        }
                    }
                }

                benchmarkResult.Values[result.Key] = groupedValues;
            }

            benchmarkResult.RaiseUpdated();
        }
    }
}