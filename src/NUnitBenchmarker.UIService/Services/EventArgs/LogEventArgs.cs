// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventArgs.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Services
{
    using System;

    public class LogEventArgs : EventArgs
    {
        #region Constructors
        public LogEventArgs(string message)
        {
            Message = message;
        }
        #endregion

        #region Properties
        public string Message { get; set; }
        #endregion
    }
}