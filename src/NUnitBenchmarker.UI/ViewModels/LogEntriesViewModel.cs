﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntriesViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Threading;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.Models;
    using NUnitBenchmarker.Services;

    public class LogEntriesViewModel : ViewModelBase
    {
        private readonly IUIServiceHost _uiServiceHost;

        private DispatcherTimer _testTimer = new DispatcherTimer();

        public LogEntriesViewModel(ICommandManager commandManager, IUIServiceHost uiServiceHost)
        {
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => uiServiceHost);

            _uiServiceHost = uiServiceHost;

            LogEntries = new ObservableCollection<LogEntry>();

            ClearLog = new Command(OnClearLogExecute);

            commandManager.RegisterCommand("Log.Clear", ClearLog, this);

            _testTimer.Interval = TimeSpan.FromSeconds(1);
            _testTimer.Tick += (sender, e) => LogEntries.Add(new LogEntry() {Message = "test item"});
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

            _testTimer.Start();
        }

        protected override void Close()
        {
            _testTimer.Stop();

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