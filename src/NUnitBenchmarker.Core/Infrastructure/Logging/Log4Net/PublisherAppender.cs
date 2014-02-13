using System;
using System.Diagnostics;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Ninject.Activation;

namespace NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net
{
	public class PublisherAppender : AppenderSkeleton
	{
		public event Action<LoggingEvent, string> LoggingEventAppended;

		protected virtual void OnLoggingEventAppended(LoggingEvent loggingEvent, string renderedMessage)
		{
			var handler = LoggingEventAppended;
			if (handler != null)
			{
				handler(loggingEvent, renderedMessage);
			}
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			OnLoggingEventAppended(loggingEvent, RenderLoggingEvent(loggingEvent));
		}

		public static PublisherAppender GetCurrent(IContext notUsed)
		{
			// Getting the appender by its type instead of by its name to get rid of the dependency how in 
			// the config file the appender is named:
			return
				(PublisherAppender)
					((Hierarchy) LogManager.GetRepository()).Root.Appenders.Cast<IAppender>()
						.FirstOrDefault(a => a.GetType() == typeof (PublisherAppender));
		}
	}
}
