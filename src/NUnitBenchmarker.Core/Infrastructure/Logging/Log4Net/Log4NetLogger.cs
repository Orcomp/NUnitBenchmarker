using System;
using log4net;
using log4net.Core;

namespace NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net
{
	public class Log4NetLogger : ILogger
	{
		private readonly ILog logger;
		//private Type callerType;
		//private string callerMemberName;

		public Log4NetLogger()
		{
			logger = LogManager.GetLogger(GetType());
		}

		public void Info(string message, params object[] args)
		{
			Log(GetType(), Level.Info, null, message, args);
		}

		public void Warn(string message, params object[] args)
		{
			Log(GetType(), Level.Warn, null, message, args);
		}

		public void Debug(string message, params object[] args)
		{
			Log(GetType(), Level.Debug, null, message, args);
		}

		public void Error(string message, params object[] args)
		{
			Log(GetType(), Level.Error, null, message, args);
		}

		public void Error(Exception e)
		{
			Log(GetType(), Level.Error, e, "");
		}

		public void Error(Exception e, string message, params object[] args)
		{
			Log(GetType(), Level.Error, e, message, e, args);
		}

		public void Fatal(string message, params object[] args)
		{
			Log(GetType(), Level.Fatal, null, message, args);
		}

		public void Fatal(Exception e)
		{
			Log(GetType(), Level.Fatal, e, "");
		}

		public void Fatal(Exception e, string message, params object[] args)
		{
			Log(GetType(), Level.Fatal, e, message, e, args);
		}

		
		private void Log(
			Type callerStackBoundaryDeclaringType,
			Level level,
			Exception exception,
			string message,
			params object[] args)
		{
			try
			{
				message = string.Format(message, args);
				logger.Logger.Log(callerStackBoundaryDeclaringType, level, message, exception);
			}
			catch(Exception e)
			{
				try
				{
					logger.Error(message, e);
				}
				catch
				{
					;
				}
			}
		}

		/// <summary>
		/// This is the most generic logginh method that is intended to be used by wrappers or remotely
		/// </summary>
		/// <param name="loggingEvent">The event being logged.</param>
		public void Log(LoggingEvent loggingEvent)
		{
			try
			{
				logger.Logger.Log(loggingEvent);
			}
			catch (Exception e)
			{
				try
				{
					logger.Error("Unexpected error while logging.", e);
				}
				catch
				{
					;
				}
			}
		}
	}
}