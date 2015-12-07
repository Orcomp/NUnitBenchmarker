// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Data;

    /// <summary>
    /// Class UIService Service definition class for exchanging data with Runner client
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class UIService : IUIService
    {
        private readonly IUIServiceHost _uiServiceHost;

        #region Constructors
        public UIService(IUIServiceHost uiServiceHost)
        {
            _uiServiceHost = uiServiceHost;
        }
        #endregion

        #region IUIService Members
        /// <summary>
        /// Sent by the client to get diagnostic ping.
        /// </summary>
        /// <param name="message">The message.</param>
        public string Ping(string message)
        {
            return _uiServiceHost.OnPing(message);
        }

        /// <summary>
        /// Logs a standard log4net logging event
        /// </summary>
        public void LogEvent(string loggingEventString)
        {
            _uiServiceHost.OnLogged(loggingEventString);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the implementations to test
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>IEnumerable{TypeSpecification}.</returns>
        public IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType)
        {
            return _uiServiceHost.OnGetImplementations(interfaceType);
        }

        public void UpdateResult(BenchmarkResult result)
        {
            _uiServiceHost.OnUpdateResult(result);
        }

        private string AnonymousToString(object @object)
        {
            if (@object == null)
            {
                return string.Empty;
            }

            var result = @object.GetType().GetProperties().Aggregate(string.Empty,
                (current, propertyInfo) => current + string.Format("'{0}': >{1}<, ", propertyInfo.Name, propertyInfo.GetValue(@object, null)));

            return result.Trim().Trim(',');
        }
        #endregion
    }
}