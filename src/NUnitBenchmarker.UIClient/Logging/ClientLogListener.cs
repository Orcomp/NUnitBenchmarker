// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogListener.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;
    using NUnitBenchmarker.UIServiceReference;

    internal class ClientLogListener : LogListenerBase
    {
        private readonly UIServiceClient _client;

        public ClientLogListener(UIServiceClient client)
        {
            _client = client;
        }

        public override void Write(ILog log, string message, LogEvent logEvent, DateTime time)
        {
            _client.LogEvent(message);
        }
    }
}