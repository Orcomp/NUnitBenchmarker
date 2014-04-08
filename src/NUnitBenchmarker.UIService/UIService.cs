// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UIService
{
    using System.Collections.Generic;
    using System.Linq;
    using Catel.IoC;
    using Catel.Logging;
    using NUnitBenchmarker.UIService.Data;

    /// <summary>
    ///     Class UIService
    ///     Service definition class for exchanging data with Runner client
    /// </summary>
    public class UIService : IUIService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IDependencyResolver _dependencyResolver;

        public UIService()
        {
            // Note: Not DI in constructor possible here because this instance is created by 
            // indirectly by .NET like this: host = new ServiceHost(typeof(UIService)) 
            // and _not_ by our configured DI container.
            // We are using DR instead of DI:

            _dependencyResolver = this.GetDependencyResolver();
        }

        /// <summary>
        ///     Sent by the client to get diagnostic ping.
        /// </summary>
        /// <param name="message">The message.</param>
        public string Ping(string message)
        {
            var host = _dependencyResolver.Resolve<IUIServiceHost>();
            return host.OnPing(message);
        }

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

        /// <summary>
        /// Logs a standard log4net logging event
        /// </summary>
        public void LogEvent(string loggingEventString)
        {
            // Note: This message is already depends on log4net via the LoggingEvent type. 
            // No additional depency is caused by accepting Log4NetLogger (specific implementation of
            // ILogger here:

            // TODO: Write log

            //LoggingEventData loggingEventData;
            //using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(loggingEventString)))
            //{
            //    var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            //    loggingEventData = ((SerializableLoggingEventData) formatter.Deserialize(ms)).ToLoggingEventData();
            //}

            //var loggingEvent = new LoggingEvent(loggingEventData);
            //((Log4NetLogger) logger).Log(loggingEvent);
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
    }
}