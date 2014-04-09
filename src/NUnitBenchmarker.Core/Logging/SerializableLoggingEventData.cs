// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableLoggingEventData.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;
    using Catel.Logging;

    /// <summary>
    /// Class SerializableLoggingEventData. Simple wrapper class for log4net's LoggingEventData which is 
    /// unfortunatelly not marked as serializable.
    /// </summary>
    [Serializable]
    [Catel.Fody.NoWeaving]
    public class SerializableLoggingEventData
    {
        public DateTime Timestamp { get; set; }

        public string Message { get; set; }

        public LogEvent LogEvent { get; set; }
    }
}