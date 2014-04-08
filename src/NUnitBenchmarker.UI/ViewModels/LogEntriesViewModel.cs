// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEntriesViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.UI.Model;

    public class LogEntriesViewModel : ViewModelBase
    {
        public LogEntriesViewModel(ICommandManager commandManager)
        {
            Argument.IsNotNull(() => commandManager);

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
        }

        protected override void Close()
        {
            base.Close();
        }
        #endregion
    }
}