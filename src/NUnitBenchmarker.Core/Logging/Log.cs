// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;

    internal class Log : ILog
    {
        public Log(Type targetType)
        {
            TargetType = targetType;
        }

        public Type TargetType { get; private set; }

        public event EventHandler<LogMessageEventArgs> LogMessage;

        public void Write(LogEvent logEvent, string message)
        {
            var logMessage = LogMessage;
            if (logMessage != null)
            {
                var eventArgs = new LogMessageEventArgs(this, string.Format("{0}", message ?? string.Empty), logEvent, DateTime.Now);
                logMessage(this, eventArgs);
            }
        }
    }
}