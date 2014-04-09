// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUIService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using NUnitBenchmarker.Data;

    [ServiceContract]
    public interface IUIService
    {
        #region Methods
        /// <summary>
        ///     Sent by the client to get diagnostic ping.
        /// </summary>
        /// <param name="message">The message.</param>
        [OperationContract]
        string Ping(string message);

        /// <summary>
        /// Gets the implementations to test
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>IEnumerable{TypeSpecification}.</returns>
        [OperationContract]
        IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType);

        /// <summary>
        /// Logs a standard log4net logging event
        /// </summary>
        [OperationContract]
        void LogEvent(string loggingEventString);

        /// <summary>
        /// Updates the result in the UI
        /// </summary>
        [OperationContract]
        void UpdateResult(BenchmarkResult result);
        #endregion
    }
}