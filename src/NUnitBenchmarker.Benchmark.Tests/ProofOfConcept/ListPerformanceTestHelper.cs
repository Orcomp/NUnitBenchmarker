// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListPerformanceTestHelper.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
    using System;
    using System.Collections.Generic;

    internal class ListPerformanceTestHelper<T>
    {
        #region Constants
        private static readonly Random Random = new Random();
        #endregion

        #region Methods
        public static IEnumerable<T> GenerateItemsToAdd(ListPerformanceTestCaseConfiguration conf)
        {
            for (int i = 0; i < conf.Size; i++)
            {
                yield return CreateNewItem();
            }
        }

        /// <summary>
        /// Creates the new item to use in performance tests.
        /// </summary>
        /// <returns>Created item</returns>
        public static T CreateNewItem()
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)Random.Next(int.MinValue, int.MaxValue);
            }

            return Activator.CreateInstance<T>();
        }

        public static IList<T> CreateListInstance(ListPerformanceTestCaseConfiguration conf)
        {
            var type = conf.TargetImplementationType.MakeGenericType(typeof(T));
            return Activator.CreateInstance(type) as IList<T>;
        }
        #endregion
    }
}