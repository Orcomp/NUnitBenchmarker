// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResultExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Collections.Generic;
    using Catel;
    using Data;

    public static class BenchmarkResultExtensions
    {
        public static void UpdateResults(this BenchmarkResult benchmarkResult, BenchmarkResult newResults)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => newResults);

            benchmarkResult.Key = newResults.Key;
            benchmarkResult.TestCases = newResults.TestCases;

            foreach (var result in newResults.Values)
            {
                var values = new List<KeyValuePair<string, double>>();

                if (benchmarkResult.Values.ContainsKey(result.Key))
                {
                    values = benchmarkResult.Values[result.Key];
                }

                foreach (var value in result.Value)
                {
                    // TODO: Sync existing values?
                    //var existingValue = (from x in values
                    //                     where string.Equals(x.Key, value.Key)
                    //                     select x).FirstOrDefault();
                    //if (!string.IsNullOrEmpty(existingValue.Key))
                    //{
                    //    existingValue[] = value.Value;
                    //}
                    //else
                    //{
                    //    values.Add(new KeyValuePair<string, double>());
                    //}

                    values.Add(new KeyValuePair<string, double>(value.Key, value.Value));
                }

                benchmarkResult.Values[result.Key] = values;
            }
        }
    }
}