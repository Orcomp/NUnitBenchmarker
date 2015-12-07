// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogManager.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class LogManager
    {
        private static readonly List<ILogListener> _logListeners = new List<ILogListener>();
        private static readonly Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();

        public static ILog GetLogger(Type type)
        {
            lock (_loggers)
            {
                if (!_loggers.ContainsKey(type))
                {
                    var log = new Log(type);
                    log.LogMessage += OnLogMessage;

                    _loggers.Add(type, log);
                }

                return _loggers[type];
            }
        }

        public static void AddListener(ILogListener listener)
        {
            lock (_logListeners)
            {
                _logListeners.Add(listener);
            }
        }

        private static void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            var logListeners = _logListeners.ToList();
            if (logListeners.Count == 0)
            {
                return;
            }

            foreach (var listener in logListeners)
            {
                listener.Write(e.Log, e.Message, e.LogEvent, e.Time);
            }
        }
    }
}