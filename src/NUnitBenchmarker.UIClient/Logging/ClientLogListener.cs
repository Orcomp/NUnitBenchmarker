// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogListener.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UIClient.Logging
{
    using System.IO;
    using System.Text;
    using Catel;
    using Catel.Logging;
    using NUnitBenchmarker.Core.Infrastructure.Logging;
    using NUnitBenchmarker.UIClient.UIServiceReference;

    public class ClientLogListener : LogListenerBase
    {
        private readonly UIServiceClient _client;

        public ClientLogListener(UIServiceClient client)
        {
            Argument.IsNotNull(() => client);

            _client = client;
        }

        protected override void Write(ILog log, string message, LogEvent logEvent, object extraData)
        {
            string loggingEventString;
            using (var memoryStream = new MemoryStream())
            {
                var data = new SerializableLoggingEventData()
                {
                    Message = message,
                    LogEvent = logEvent
                };

                var formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
                formatter.Serialize(memoryStream, data);
                loggingEventString = Encoding.UTF8.GetString(memoryStream.ToArray());
            }

            _client.Log(loggingEventString);
        }
    }
}