// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogMessageEventArgs.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;

    internal class LogMessageEventArgs : EventArgs
    {
        public LogMessageEventArgs(ILog log, string message, object extraData,LogEvent logEvent)
            : this(log, message, logEvent, DateTime.Now)
        { }

        public LogMessageEventArgs(ILog log, string message, LogEvent logEvent, DateTime time)
        {
            Log = log;
            Time = time;
            Message = message;
            LogEvent = logEvent;
        }

        public ILog Log { get; private set; }

        public string Message { get; private set; }

        public LogEvent LogEvent { get; private set; }

        public DateTime Time { get; private set; }
    }
}