// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublisherAppenderTest.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Core.Tests.Logging
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class PublisherAppenderTest
    {
        //[Test]
        //public void ConfigurationTest()
        //{
        //    var logger = LogManager.GetCurrentClassLogger();
        //    var testMessage = string.Format("Test message: {0}", DateTime.Now.Ticks);

        //    var publisherAppender = PublisherAppender.GetCurrent(null);
        //    publisherAppender.LoggingEventAppended += (@event, mssage) =>
        //    {
        //        Assert.IsTrue(true); // Prevent ReSharper to inline lambda
        //        Assert.IsTrue(@event.MessageObject.ToString().Contains(testMessage));
        //    };

        //    Act(look for Assert in the event handler lambda below)
        //    logger.Info(testMessage);
        //}
}
}