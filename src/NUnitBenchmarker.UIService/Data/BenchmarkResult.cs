// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BenchmarkResult.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#define DATACONTRACTS

namespace NUnitBenchmarker.Data
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class BenchmarkResult
    {
        public BenchmarkResult()
        {
            Values = new Dictionary<string, List<KeyValuePair<string, double>>>();
        }

        #region Properties
        [DataMember]
        public TypeSpecification TypeSpecification { get; set; }

		[DataMember]
		public TypeSpecification InterfaceSpecification { get; set; }


        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public Dictionary<string, List<KeyValuePair<string, double>>> Values { get; set; }

        [DataMember]
        public string[] TestCases { get; set; }
        #endregion

        #region Events
        public event EventHandler<EventArgs> Updated;
        #endregion

        #region Methods
        public void RaiseUpdated()
        {
            var handler = Updated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        #endregion

    }
} 