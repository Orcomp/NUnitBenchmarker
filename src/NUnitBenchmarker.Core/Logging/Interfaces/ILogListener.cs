// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogListener.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;

    internal interface ILogListener
    {
        void Write(ILog log, string message, LogEvent logEvent, DateTime time);
    }
}