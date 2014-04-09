// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUIServiceHost.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using NUnitBenchmarker.Data;

    public interface IUIServiceHost
    {
        #region Methods
        void OnLogged(string message);
        string OnPing(string message);

        IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification interfaceType);

        void OnUpdateResult(BenchmarkResult result);

        /// <summary>
        /// Starts the Runner - UI communication service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the Runner - UI communication service.
        /// </summary>
        void Stop();
        #endregion

        event Action<string> Logged;
        event Func<string, string> Ping;
        event Func<TypeSpecification, IEnumerable<TypeSpecification>> GetImplementations;
        event Action<BenchmarkResult> UpdateResult;
    }
}