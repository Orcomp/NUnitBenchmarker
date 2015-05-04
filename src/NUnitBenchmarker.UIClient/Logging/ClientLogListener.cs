// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogListener.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.Logging
{
    using System;
    using Catel;
    using Catel.Logging;
    using NUnitBenchmarker.UIServiceReference;

    public class ClientLogListener : LogListenerBase
    {
        private readonly UIServiceClient _client;

        public ClientLogListener(UIServiceClient client)
        {
            Argument.IsNotNull(() => client);

            _client = client;

            IgnoreCatelLogging = true;
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData, DateTime time)
        {
            _client.LogEvent(message);
        }
    }
}