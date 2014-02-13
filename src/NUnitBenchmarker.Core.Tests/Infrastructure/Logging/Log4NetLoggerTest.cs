using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net;

namespace NUnitBenchmarker.Core.Tests.Infrastructure.Logging
{
	[TestFixture]
	public class Log4NetLoggerTest
	{
		// [assembly: log4net.Config.XmlConfigurator(ConfigFile = "NUnitBenchmarker.log4net.config", Watch = true)]
		// placed in NUnitBenchmarker.Core assembly, now testing if simply referencing it makes the logging work.
		// NOTE: The NUnitBenchmarker.log4net.config project item is configured to be copied to the output folder when building
		[Test]
		public void ConfigurationTest()
		{
			var testMessage = string.Format("Test message: {0}", DateTime.Now.Ticks);
			const string logFileName = "NUnitBenchmarker.log";
			const string logFileCopyName = "logCopy.txt";

			try
			{
				File.Delete(logFileCopyName);
			}
			catch (FileNotFoundException)
			{
				;
			}

			var logger = new Log4NetLogger();
			logger.Info(testMessage);

			File.Copy(logFileName, logFileCopyName);

			var loggedText = File.ReadAllText(logFileCopyName);
			Debug.WriteLine(loggedText);
			Debug.WriteLine(testMessage);

			Assert.IsTrue(loggedText.Contains(testMessage));
		}

		public void LocationInfoTest()
		{

		}
	}
}
