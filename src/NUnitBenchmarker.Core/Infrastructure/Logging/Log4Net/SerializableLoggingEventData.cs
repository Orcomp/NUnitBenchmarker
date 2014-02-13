using System;
using log4net.Core;
using log4net.Util;

namespace NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net
{
	/// <summary>
	/// Class SerializableLoggingEventData. Simple wrapper class for log4net's LoggingEventData which is 
	/// unfortunatelly not marked as serializable.
	/// </summary>
	[Serializable]
	public class SerializableLoggingEventData
	{
		public string LoggerName;
		public Level Level;
		public string Message;
		public string ThreadName;
		public DateTime TimeStamp;
		public LocationInfo LocationInfo;
		public string UserName;
		public string Identity;
		public string ExceptionString;
		public string Domain;
		public PropertiesDictionary Properties;

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializableLoggingEventData"/> class with the 
		/// original log4net logging data
		/// </summary>
		/// <param name="loggingEventData">The original logging event data.</param>
		public SerializableLoggingEventData(LoggingEventData loggingEventData)
		{
			LoggerName = loggingEventData.LoggerName;
			Level = loggingEventData.Level;
			Message = loggingEventData.Message;
			ThreadName = loggingEventData.ThreadName;
			TimeStamp = loggingEventData.TimeStamp;
			LocationInfo = loggingEventData.LocationInfo;
			UserName = loggingEventData.UserName;
			Identity = loggingEventData.Identity;
			ExceptionString = loggingEventData.ExceptionString;
			Domain = loggingEventData.Domain;
			Properties = loggingEventData.Properties;
		}

		/// <summary>
		/// Instantiates an initialized log4net logging event data with the current self state
		/// </summary>
		/// <returns>LoggingEventData.</returns>
		public LoggingEventData ToLoggingEventData()
		{
			var loggingEventData = new LoggingEventData();
			loggingEventData.LoggerName = LoggerName;
			loggingEventData.Level = Level;
			loggingEventData.Message = Message;
			loggingEventData.ThreadName = ThreadName;
			loggingEventData.TimeStamp = TimeStamp;
			loggingEventData.LocationInfo = LocationInfo;
			loggingEventData.UserName = UserName;
			loggingEventData.Identity = Identity;
			loggingEventData.ExceptionString = ExceptionString;
			loggingEventData.Domain = Domain;
			loggingEventData.Properties = Properties;

			return loggingEventData;
		}
	}
}