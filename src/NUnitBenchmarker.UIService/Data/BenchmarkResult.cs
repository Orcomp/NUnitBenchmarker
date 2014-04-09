// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResult.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Data
{
    using System.Collections.Generic;
    using Catel.Data;

    //[DataContract]
    public class BenchmarkResult : ModelBase
    {
        public BenchmarkResult()
        {
            Values = new Dictionary<string, List<KeyValuePair<string, double>>>();
        }

        #region Properties
        //[DataMember]
        public string Key { get; set; }

        //[DataMember]
        public Dictionary<string, List<KeyValuePair<string, double>>> Values { get; set; }

        //[DataMember]
        public string[] TestCases { get; set; }
        #endregion
    }
} 