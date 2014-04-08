// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableLoggingEventData.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Core.Infrastructure.Logging
{
    using System;
    using Catel.Data;
    using Catel.Logging;

    /// <summary>
    /// Class SerializableLoggingEventData. Simple wrapper class for log4net's LoggingEventData which is 
    /// unfortunatelly not marked as serializable.
    /// </summary>
    [Serializable]
    public class SerializableLoggingEventData : ModelBase
    {
        public string Message { get; set; }

        public LogEvent LogEvent { get; set; }
    }
}