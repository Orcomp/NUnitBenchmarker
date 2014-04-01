using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Xml.Serialization;
using log4net.Core;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net;
using NUnitBenchmarker.UIClient.Properties;
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
			if (!DisplayUI)
			{
				return;
			}

			string loggingEventString;
			using (var memoryStream = new MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
				formatter.Serialize(memoryStream, new SerializableLoggingEventData(loggingEvent.GetLoggingEventData()));
				loggingEventString = Encoding.UTF8.GetString(memoryStream.ToArray());
			}

			if (Start(false))
			{
				Client.Log(loggingEventString);
				return;
			}
			// Note: We can not log here. We are in an appender....
		}

		public static ILogger Logger { get; private set; }
		public static bool DisplayUI { get; set; }

		public static string Ping(string message)
	    {
			return SendMessageFunc(() => Client.Ping(message), string.Format(Resources.UI_Communication_welcome_to_the_loopback, message));
	    }

		public static IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType)
		{
			return SendMessageFunc<IEnumerable<TypeSpecification>>(() => Client.GetImplementations(interfaceType), new List<TypeSpecification>());
		}

		public static void UpdateResult(BenchmarkResult result)
		{
			SendMessageAction(()=>Client.UpdateResult(result));
		}

#if NET45		
		private static void SendMessageAction(Action action, [CallerMemberName] string memberName = "")
#else
		private static void SendMessageAction(Action action, string memberName = "")
#endif
		{
			if (!DisplayUI)
			{
				return;
			}

			if (Start())
			{
				action();
				return;
			}
			
			Logger.Warn(Resources.UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message, memberName);
		}

		private static T SendMessageFunc<T>(Func<T> func, T defaultResult, string memberName = "")
		{
			if (!DisplayUI)
			{
				return defaultResult;
			}

			if (Start())
			{
				return func();
			}
			Logger.Warn(Resources.UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message, memberName);
			return defaultResult;
		}

		
		private static string uiExeName = "NUnitBenchmarker.UI.exe";
		private static string uiProcessName = "..\\NUnitBenchmarker.UI\\" + uiExeName;
		private static string uiProcessToolFolder = "..\\NUnitBenchmarker.UI\\" + uiExeName;
		
		// TODO: Make this thread safe (possibly involves refactor UI class from static to instance)

		public static string GetUiProcessName()
		{
			if (File.Exists(uiProcessName))
			{
				return uiProcessName;
			}

			string result;
			string start = "";
			for (int i = 0; i < 10; i++, start += @"..\")
			{
				if (null != (result = GetUiProcessName(start)))
				{
					return result;
				}
			}
			return null;
		}

		private static string GetUiProcessName(string start)
		{
			string startFolder = start;
			if (!Directory.Exists(startFolder))
			{
				return null;
			}

			var di = new DirectoryInfo(startFolder);
			var files = di.GetFiles(uiExeName, SearchOption.AllDirectories).Where(fi=> fi.FullName.ToLower().Contains("packages")).ToArray();

			if (files.Length != 0)
			{
				return files[0].FullName;
			}
			return null;
		}



		public static bool Start(bool forceStart = true)
		{
			if (!DisplayUI)
			{
				return true;
			}

			uiProcessName = GetUiProcessName();


			var process = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(uiProcessName)).FirstOrDefault();
			var starting = false;
			var timeout = 500; // Normal check timout

			// TODO: Implement using full path. Currently the UI executable must be copied to the test directory
			if (process == null)
			{
				if (!forceStart)
				{
					return false;
				}
				starting = true;
				timeout = 5000; // Start timeout
				try
				{
					process = Process.Start(uiProcessName);
				}
				catch (Exception e)
				{
					Logger.Error(e, Resources.UI_Message_can_not_start_ui_process);
					return false;
				}
				Logger.Info(Resources.UI_Message_starting_ui_process);
			}
			if (process == null)
			{
				Logger.Info(Resources.UI_Message_starting_ui_process);
				return false;
			}

			// Wait to start (or check if responding):
			bool success = process.WaitForInputIdle(timeout);


			// Log only if process just was started:
			if (starting)
			{
				if (success)
				{
					Logger.Info(Resources.UI_Message_start_ui_process_successfully_started);
				}
				else
				{
					Logger.Error(Resources.UI_Message_can_not_start_ui_process);
				}
			}
			else
			{
				SetForegroundWindow(process.MainWindowHandle);
			}
			return success;

		}
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
