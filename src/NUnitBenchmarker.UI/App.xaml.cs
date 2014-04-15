// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using System.Threading;
    using System.Windows;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.MVVM;
    using NUnitBenchmarker.Models;
    using NUnitBenchmarker.Services;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Methods
        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.
        /// </param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener(true);
#endif

            Catel.Data.ModelBase.DefaultSuspendValidationValue = true;

            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            try
            {
                var serviceLocator = ServiceLocator.Default;

                RegisterServices(serviceLocator);
                RegisterCommands(serviceLocator.ResolveType<ICommandManager>());

                // This is only for verbose logging info and debug info
                Thread.CurrentThread.Name = "UI";

                // Informational log message
                Log.Info("Application started");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            base.OnStartup(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="T:System.Windows.ExitEventArgs" /> that contains the event data.
        /// </param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            try
            {
                // Informational log message
                Log.Info("Application terminated normally.");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void RegisterServices(IServiceLocator serviceLocator)
        {
            serviceLocator.RegisterType<IReflectionService, ReflectionService>();
            serviceLocator.RegisterType<IUIServiceHost, UIServiceHost>();
            serviceLocator.RegisterType<ITestTargetService, TestTargetService>();
            serviceLocator.RegisterType<ISettings, Settings>();
        }

        private void RegisterCommands(ICommandManager commandManager)
        {
            commandManager.CreateCommand("File.Open");
            commandManager.CreateCommand("File.SaveAllResults");
            commandManager.CreateCommand("File.Exit");

            commandManager.CreateCommand("Log.Clear");

            commandManager.RegisterAction("File.Exit", Shutdown);
        }
        #endregion
    }
}