// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogListenerBase.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;

    internal abstract class LogListenerBase : ILogListener
    {
        public abstract void Write(ILog log, string message, LogEvent logEvent, DateTime time);
    }
}