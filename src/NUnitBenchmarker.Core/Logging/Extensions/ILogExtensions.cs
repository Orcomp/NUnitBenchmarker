// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    internal static class ILogExtensions
    {
        public static void Error(this ILog log, string messageFormat, params object[] args)
        {
            log.Write(LogEvent.Error, messageFormat, args);
        }

        public static void Warning(this ILog log, string messageFormat, params object[] args)
        {
            log.Write(LogEvent.Warning, messageFormat, args);
        }

        public static void Info(this ILog log, string messageFormat, params object[] args)
        {
            log.Write(LogEvent.Info, messageFormat, args);
        }

        public static void Debug(this ILog log, string messageFormat, params object[] args)
        {
            log.Write(LogEvent.Debug, messageFormat, args);
        }

        public static void Write(this ILog log, LogEvent logEvent, string messageFormat, params object[] args)
        {
            log.Write(logEvent, string.Format(messageFormat, args));
        }
    }
}