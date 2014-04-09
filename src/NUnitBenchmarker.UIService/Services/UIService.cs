// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using NUnitBenchmarker.Data;

    /// <summary>
    /// Class UIService Service definition class for exchanging data with Runner client
    /// </summary>
    public class UIService : IUIService
    {
        #region Fields
        private readonly IDependencyResolver _dependencyResolver;
        #endregion

        #region Constructors
        public UIService()
        {
            // Note: Not DI in constructor possible here because this instance is created by 
            // indirectly by .NET like this: host = new ServiceHost(typeof(UIService)) 
            // and _not_ by our configured DI container.
            // We are using DR instead of DI:

            _dependencyResolver = this.GetDependencyResolver();
        }
        #endregion

        #region Events
        public event EventHandler<LogEventArgs> Logged;
        #endregion

        #region IUIService Members
        /// <summary>
        /// Sent by the client to get diagnostic ping.
        /// </summary>
        /// <param name="message">The message.</param>
        public string Ping(string message)
        {
            var host = _dependencyResolver.Resolve<IUIServiceHost>();
            return host.OnPing(message);
        }

        /// <summary>
        /// Logs a standard log4net logging event
        /// </summary>
        public void LogEvent(string loggingEventString)
        {
            Logged.SafeInvoke(this, new LogEventArgs(loggingEventString));
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
            var host = _dependencyResolver.Resolve<IUIServiceHost>();
            return host.OnGetImplementations(interfaceType);
        }

        public void UpdateResult(BenchmarkResult result)
        {
            var host = _dependencyResolver.Resolve<IUIServiceHost>();
            host.OnUpdateResult(result);
        }

        private string AnonymousToString(object @object)
        {
            if (@object == null)
            {
                return string.Empty;
            }

            string result = @object.GetType().GetProperties().Aggregate(string.Empty,
                (current, propertyInfo) => current + string.Format("'{0}': >{1}<, ", propertyInfo.Name, propertyInfo.GetValue(@object, null)));

            return result.Trim().Trim(',');
        }
        #endregion
    }
}