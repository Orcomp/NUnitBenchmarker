// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILog.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;

    internal interface ILog
    {
        event EventHandler<LogMessageEventArgs> LogMessage;

        void Write(LogEvent logEvent, string message);
    }
}