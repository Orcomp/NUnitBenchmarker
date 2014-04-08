using System;
using NUnit.Framework;
using NUnitBenchmarker.Core.Infrastructure.Logging;

namespace NUnitBenchmarker.Core.Tests.Infrastructure.Logging
{
    using Catel.Logging;

    [TestFixture]
	public class PublisherAppenderTest
	{
		[Test]
		public void ConfigurationTest()
		{
			var logger = LogManager.GetCurrentClassLogger();
			var testMessage = string.Format("Test message: {0}", DateTime.Now.Ticks);


            //var publisherAppender = PublisherAppender.GetCurrent(null);
            //publisherAppender.LoggingEventAppended += (@event, mssage) =>
            //{
            //    Assert.IsTrue(true); // Prevent ReSharper to inline lambda
            //    Assert.IsTrue(@event.MessageObject.ToString().Contains(testMessage));
            //};
			
			// Act (look for Assert in the event handler lambda below)
			logger.Info(testMessage);
		}
	}
}
