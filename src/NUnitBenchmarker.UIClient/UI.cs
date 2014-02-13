using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using log4net.Core;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net;
using NUnitBenchmarker.UIClient.UIServiceReference;
using NUnitBenchmarker.UIService.Data;
using ILogger = NUnitBenchmarker.Core.Infrastructure.Logging.ILogger;

namespace NUnitBenchmarker.UIClient
{
    // TODO: Refactor this static helper to an instance which implements IUIService
	public static class UI
    {
	    private static readonly UIServiceClient Client;

	    static UI()
	    {
			string resourceStreamName = string.Format("NUnitBenchmarker.UIClient.{0}", "UIServiceClient.config");
			string configXml = string.Empty;
			
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceStreamName))
			{
				if (stream != null)
				{
					using (var reader = new StreamReader(stream))
					{
						configXml = reader.ReadToEnd();
					}
				}
			}
			const string endpointName = "BasicHttpBinding_IUIService";

			var serviceEndpoint = new XmlServiceEndpoint(typeof(IUIService), configXml, endpointName);
			var channelFactory = new ChannelFactory<IUIServiceChannel>(serviceEndpoint);
		    Client = new UIServiceClient(channelFactory.Endpoint.Binding, channelFactory.Endpoint.Address);

			Logger = new Log4NetLogger();
		    var publisherAppender = PublisherAppender.GetCurrent(null);
		    publisherAppender.LoggingEventAppended += LoggingEventAppended;

	    }

		private static void LoggingEventAppended(LoggingEvent loggingEvent, string renderedMessage)
		{
			string loggingEventString;
			using (var memoryStream = new MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
				formatter.Serialize(memoryStream, new SerializableLoggingEventData(loggingEvent.GetLoggingEventData()));
				loggingEventString = Encoding.UTF8.GetString(memoryStream.ToArray());
			}

			Client.Log(loggingEventString);
		}

		public static ILogger Logger { get; private set; }

		public static string Ping(string message)
	    {
		    return Client.Ping(message);
	    }

		public static IEnumerable<string> GetAssemblyNames()
		{
			return Client.GetAssemblyNames();
		}

		public static void UpdateResult(BenchmarkResult result)
		{
			Client.UpdateResult(result);
		}


	    public static void Start()
	    {
		    throw new NotImplementedException();
	    }

		
    }
}
