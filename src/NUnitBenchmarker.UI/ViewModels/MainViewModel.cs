// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Data;
    using Models;
    using Resources;
    using Services;
    using System.Threading.Tasks;
    using Catel.Reflection;

    /// <summary>
    /// Class MainViewModel: MVVM ViewModel for MainWindow
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Fields
        private readonly IUIServiceHost _uiServiceHost;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel(IUIServiceHost uiServiceHost, IMessageService messageService,
            ISelectDirectoryService selectDirectoryService, ICommandManager commandManager, ISettings settings)
        {
            Argument.IsNotNull(() => uiServiceHost);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => settings);

            _uiServiceHost = uiServiceHost;
            _messageService = messageService;
            _selectDirectoryService = selectDirectoryService;
            Settings = settings;

            BenchmarkResults = new ObservableCollection<BenchmarkResult>();

            CloseBenchmarkResult = new Command<BenchmarkResult>(OnCloseBenchmarkResultExecute);
            SwitchTimeAxis = new Command(OnSwitchTimeAxisExecute);
            SaveAllResults = new TaskCommand(OnSaveAllResultsExecuteAsync);
            ChangeDefaultAxis = new Command(OnChangeDefaultAxisExecute);

            commandManager.RegisterCommand(Commands.File.SaveAllResults, SaveAllResults, this);
            commandManager.RegisterCommand(Commands.Options.ChangeDefaultAxis, ChangeDefaultAxis, this);
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return string.Format("NUnit Benchmarker v{0}", Assembly.GetExecutingAssembly().InformationalVersion()); }
        }

        public string LastPingMessage { get; set; }

        public ISettings Settings { get; private set; }

        public ObservableCollection<BenchmarkResult> BenchmarkResults { get; private set; }

        public BenchmarkResult SelectedBenchmarkResult { get; set; }
        #endregion

        #region Commands
        public Command<BenchmarkResult> CloseBenchmarkResult { get; private set; }

        private void OnCloseBenchmarkResultExecute(BenchmarkResult result)
        {
            BenchmarkResults.Remove(result);
        }

        public Command SwitchTimeAxis { get; private set; }

        private void OnSwitchTimeAxisExecute()
        {
            // TODO: Handle command logic here
        }

        public TaskCommand SaveAllResults { get; private set; }

        private async Task OnSaveAllResultsExecuteAsync()
        {
            if (_selectDirectoryService.DetermineDirectory())
            {
                var folderName = _selectDirectoryService.DirectoryName;
                var failed = false;

                try
                {
                    Benchmarker.ExportAllResults(BenchmarkResults.ToList(), folderName);
                }
                catch (Exception ex)
                {
                    Log.Error(ex);

                    failed = true;
                }

                if (failed)
                {
                    await _messageService.ShowErrorAsync(string.Format(UIStrings.MainViewModel_SaveAllResultsAction_Error_saving_results_));
                }
            }
        }

        public Command ChangeDefaultAxis { get; private set; }

        private void OnChangeDefaultAxisExecute()
        {
            Settings.Save();
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            try
            {
                _uiServiceHost.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            _uiServiceHost.Ping += OnPing;
            _uiServiceHost.UpdateResult += OnUpdateResult;
        }

        protected override async Task CloseAsync()
        {
            _uiServiceHost.Stop();

            _uiServiceHost.Ping -= OnPing;
            _uiServiceHost.UpdateResult -= OnUpdateResult;

            await base.CloseAsync();
        }

        private string OnPing(string message)
        {
            LastPingMessage = message;

            return string.Format("Welcome to the machine: {0}", message);
        }

        private void OnUpdateResult(BenchmarkResult result)
        {
            var currentItem = (from x in BenchmarkResults
                               where string.Equals(x.Key, result.Key)
                               select x).FirstOrDefault();
            if (currentItem != null)
            {
                currentItem.UpdateResults(result);
                return;
            }

            BenchmarkResults.Add(result);

            if (SelectedBenchmarkResult == null)
            {
                SelectedBenchmarkResult = result;
            }
        }
        #endregion
    }
}