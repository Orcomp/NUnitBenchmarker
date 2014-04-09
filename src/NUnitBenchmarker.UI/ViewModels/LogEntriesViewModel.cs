// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntriesViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.Models;
    using NUnitBenchmarker.Services;

    public class LogEntriesViewModel : ViewModelBase
    {
        private readonly IUIServiceHost _uiServiceHost;

        public LogEntriesViewModel(ICommandManager commandManager, IUIServiceHost uiServiceHost)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => uiServiceHost);

            _uiServiceHost = uiServiceHost;

            LogEntries = new ObservableCollection<LogEntry>();

            ClearLog = new Command(OnClearLogExecute);

            commandManager.RegisterCommand("Log.Clear", ClearLog, this);
        }

        #region Properties
        public ObservableCollection<LogEntry> LogEntries { get; private set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the ClearLog command.
        /// </summary>
        public Command ClearLog { get; private set; }

        /// <summary>
        /// Method to invoke when the ClearLog command is executed.
        /// </summary>
        private void OnClearLogExecute()
        {
            LogEntries.Clear();
        }

        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            _uiServiceHost.Logged += OnLogged;
        }

        protected override void Close()
        {
            _uiServiceHost.Logged -= OnLogged;

            base.Close();
        }

        private void OnLogged(string message)
        {
            var logEntry = new LogEntry();
            logEntry.Message = message;

            LogEntries.Add(logEntry);
        }
        #endregion
    }
}