// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEvent.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    public enum LogEvent
    {
        /// <summary>
        ///   Debug message.
        /// </summary>
        Debug = 1,

        /// <summary>
        ///   Info message.
        /// </summary>
        Info = 2,

        /// <summary>
        ///   Warning message.
        /// </summary>
        Warning = 4,

        /// <summary>
        ///   Error message.
        /// </summary>
        Error = 8
    }
}