// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublisherAppender.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Core.Infrastructure.Logging
{
    using Catel.Logging;

    public class PublisherAppender : LogListenerBase
    {
        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData)
        {
            base.Write(log, message, logEvent, extraData);
        }
    }
}