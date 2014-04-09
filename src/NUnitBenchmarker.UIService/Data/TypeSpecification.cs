// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeSpecification.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace NUnitBenchmarker.Data
{
    using System;

    using System.Runtime.Serialization;

    [DataContract]
    public class TypeSpecification
    {
        #region Constructors
        public TypeSpecification()
        {
        }

        public TypeSpecification(Type type)
        {
            FullName = type.FullName;
            AssemblyPath = type.Assembly.CodeBase;
        }
        #endregion

        #region Properties
        [DataMember]
        public string AssemblyPath { get; set; }

        [DataMember]
        public string FullName { get; set; }
        #endregion
    }
}