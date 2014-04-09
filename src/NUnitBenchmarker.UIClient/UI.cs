// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UI.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.ServiceModel;
    using Catel.Logging;
    using NUnitBenchmarker.Logging;
    using NUnitBenchmarker.Properties;
    using NUnitBenchmarker.UIServiceReference;
    using IUIService = NUnitBenchmarker.Services.IUIService;
    using BenchmarkResult = NUnitBenchmarker.Data.BenchmarkResult;
    using TypeSpecification = NUnitBenchmarker.Data.TypeSpecification;

    // TODO: Refactor this static helper to an instance which implements IUIService
    public static class UI
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly UIServiceClient Client;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private const string uiExeName = "NUnitBenchmarker.UI.exe";
        private static string _uiProcessName = "..\\NUnitBenchmarker.UI\\" + uiExeName;

        static UI()
        {
            string resourceStreamName = string.Format("NUnitBenchmarker.UIClient.{0}", "UIServiceClient.config");
            string configXml = string.Empty;

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceStreamName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        configXml = reader.ReadToEnd();
                    }
                }
            }

            const string endpointName = "BasicHttpBinding_IUIService";

            var serviceEndpoint = new XmlServiceEndpoint(typeof (IUIService), configXml, endpointName);
            var channelFactory = new ChannelFactory<IUIServiceChannel>(serviceEndpoint);
            Client = new UIServiceClient(channelFactory.Endpoint.Binding, channelFactory.Endpoint.Address);
        }

        public static bool DisplayUI { get; set; }

        public static string Ping(string message)
        {
            return SendMessageFunc(() => Client.Ping(message), string.Format(Resources.UI_Communication_welcome_to_the_loopback, message));
        }

        public static IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType)
        {
            return SendMessageFunc<IEnumerable<TypeSpecification>>(() => Client.GetImplementations(interfaceType), new List<TypeSpecification>());
        }

        public static void UpdateResult(BenchmarkResult result)
        {
            SendMessageAction(() => Client.UpdateResult(result));
        }

#if NET45
        private static void SendMessageAction(Action action, [CallerMemberName] string memberName = "")
#else
        private static void SendMessageAction(Action action, string memberName = "")
#endif
        {
            if (!DisplayUI)
            {
                return;
            }

            if (Start())
            {
                action();
                return;
            }

            Log.Warning(Resources.UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message, memberName);
        }

        private static T SendMessageFunc<T>(Func<T> func, T defaultResult, string memberName = "")
        {
            if (!DisplayUI)
            {
                return defaultResult;
            }

            if (Start())
            {
                return func();
            }

            Log.Warning(Resources.UI_Message_can_not_start_or_contact_ui_process_when_trying_to_send_message, memberName);

            return defaultResult;
        }

        // TODO: Make this thread safe (possibly involves refactor UI class from static to instance)

        public static string GetUiProcessName()
        {
            if (File.Exists(_uiProcessName))
            {
                return _uiProcessName;
            }

            string result;
            string start = string.Empty;
            for (int i = 0; i < 10; i++, start += @"..\")
            {
                if (null != (result = GetUiProcessName(start)))
                {
                    return result;
                }
            }
            return null;
        }

        private static string GetUiProcessName(string start)
        {
            string startFolder = start;
            if (!Directory.Exists(startFolder))
            {
                return null;
            }

            var di = new DirectoryInfo(startFolder);
            var files = di.GetFiles(uiExeName, SearchOption.AllDirectories).Where(fi => fi.FullName.ToLower().Contains("packages")).ToArray();

            if (files.Length != 0)
            {
                return files[0].FullName;
            }
            return null;
        }

        public static bool Start(bool forceStart = true)
        {
            if (!DisplayUI)
            {
                return true;
            }

            LogManager.AddListener(new ClientLogListener(Client));

            _uiProcessName = GetUiProcessName();

            var process = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_uiProcessName)).FirstOrDefault();
            var starting = false;
            var timeout = 500; // Normal check timout

            if (process == null)
            {
                if (!forceStart)
                {
                    return false;
                }

                starting = true;
                timeout = 5000; // Start timeout

                try
                {
                    process = Process.Start(_uiProcessName);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Resources.UI_Message_can_not_start_ui_process);
                    return false;
                }

                Log.Info(Resources.UI_Message_starting_ui_process);
            }

            if (process == null)
            {
                Log.Info(Resources.UI_Message_starting_ui_process);
                return false;
            }

            // Wait to start (or check if responding):
            bool success = process.WaitForInputIdle(timeout);

            // Log only if process just was started:
            if (starting)
            {
                if (success)
                {
                    Log.Info(Resources.UI_Message_start_ui_process_successfully_started);
                }
                else
                {
                    Log.Error(Resources.UI_Message_can_not_start_ui_process);
                }
            }
            else
            {
                SetForegroundWindow(process.MainWindowHandle);
            }

            return success;
        }
    }
}