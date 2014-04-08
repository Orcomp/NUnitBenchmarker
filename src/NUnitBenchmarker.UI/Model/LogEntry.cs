// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntry.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.Model
{
    using System;
    using Catel.Data;
    using Catel.Logging;

    public class LogEntry : ModelBase
    {
        public LogEntry()
        {
            SuspendValidation = true;
        }

        #region Properties
        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public LogEvent LogEvent { get; set; }
        #endregion
    }
}