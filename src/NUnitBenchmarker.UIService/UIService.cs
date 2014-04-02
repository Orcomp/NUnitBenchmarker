using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using log4net.Core;
using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net;
using NUnitBenchmarker.UIService.Data;
using ILogger = NUnitBenchmarker.Core.Infrastructure.Logging.ILogger;

namespace NUnitBenchmarker.UIService
{
	/// <summary>
	///     Class UIService
	///     Service definition class for exchanging data with Runner client
	/// </summary>
	public class UIService : IUIService
	{
		private ILogger logger;

		public UIService()
		{
			// Note: Not DI in constructor possible here because this instance is created by 
			// indirectly by .NET like this: host = new ServiceHost(typeof(UIService)) 
			// and _not_ by our configured DI container.
			// We are using DR instead of DI:
			logger = Dependency.Resolve<ILogger>();
		}

		/// <summary>
		///     Sent by the client to get diagnostic ping.
		/// </summary>
		/// <param name="message">The message.</param>
		public string Ping(string message)
		{
			LogCall(new {message});
			
			var host = Dependency.Resolve<IUIServiceHost>();
			return host.OnPing(message);			
		}


		/// <summary>
		/// Gets the implementations to test
		/// </summary>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>IEnumerable{TypeSpecification}.</returns>
		public IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType)
		{
			LogCall(null);
			var host = Dependency.Resolve<IUIServiceHost>();
			return host.OnGetImplementations(interfaceType);	
		}

		/// <summary>
		/// Logs a standard log4net logging event
		/// </summary>
		public void Log(string loggingEventString)
		{
			if (!(logger is Log4NetLogger))
			{
				logger.Info(loggingEventString);
				return;
			}
			// Note: This message is already depends on log4net via the LoggingEvent type. 
			// No additional depency is caused by accepting Log4NetLogger (specific implementation of
			// ILogger here:
			
			LoggingEventData loggingEventData;
			using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(loggingEventString))) 
			{
			    var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
				loggingEventData = ((SerializableLoggingEventData)formatter.Deserialize(ms)).ToLoggingEventData();
				
			}
			var loggingEvent = new LoggingEvent(loggingEventData);
			
			((Log4NetLogger)logger).Log(loggingEvent);	
			
		}

		public void UpdateResult(BenchmarkResult result)
		{
			LogCall(new { result.Key });
			var host = Dependency.Resolve<IUIServiceHost>();
			host.OnUpdateResult(result);
		}

#if NET45
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogCall( object parameters, [CallerMemberName] string memberName = "" )
#else
		private void LogCall( object parameters, string memberName = "" )
#endif
		{
			// Dependency.Resolve<ILogger>().Info("UIService command '{0}' was received with the following parameters: {1}.", memberName, AnonymousToString(parameters));
		}



		private string AnonymousToString(object @object)
		{
			if (@object == null)
			{
				return string.Empty;
			}
			string result = @object.GetType().GetProperties().Aggregate(
				string.Empty, 
				(current, propertyInfo) 
					=> 
				current + string.Format("'{0}': >{1}<, ", propertyInfo.Name, propertyInfo.GetValue(@object, null)));

			return result.Trim().Trim(',');
		}

		
	}
}