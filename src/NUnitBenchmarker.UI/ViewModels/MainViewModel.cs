// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
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
            SaveAllResults = new Command(OnSaveAllResultsExecute);
            ChangeDefaultAxis = new Command(OnChangeDefaultAxisExecute);

            commandManager.RegisterCommand(Commands.File.SaveAllResults, SaveAllResults, this);
            commandManager.RegisterCommand(Commands.Options.ChangeDefaultAxis, ChangeDefaultAxis, this);
        }

        private void OnChangeDefaultAxisExecute()
        {
            Settings.Save();
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return string.Format("NUnit Benchmarker v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
        }

        public string LastPingMessage { get; set; }

        public ISettings Settings { get; private set; }

        public ObservableCollection<BenchmarkResult> BenchmarkResults { get; private set; }

        public BenchmarkResult SelectedBenchmarkResult { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the CloseBenchmarkResult command.
        /// </summary>
        public Command<BenchmarkResult> CloseBenchmarkResult { get; private set; }

        /// <summary>
        /// Method to invoke when the CloseBenchmarkResult command is executed.
        /// </summary>
        private void OnCloseBenchmarkResultExecute(BenchmarkResult result)
        {
            BenchmarkResults.Remove(result);
        }

        /// <summary>
        /// Gets the SwitchTimeAxis command.
        /// </summary>
        public Command SwitchTimeAxis { get; private set; }

        /// <summary>
        /// Method to invoke when the SwitchTimeAxis command is executed.
        /// </summary>
        private void OnSwitchTimeAxisExecute()
        {
            // TODO: Handle command logic here
        }

        /// <summary>
        /// Gets the SaveAllResults command.
        /// </summary>
        public Command SaveAllResults { get; private set; }

        public Command ChangeDefaultAxis { get; private set; }

        /// <summary>
        /// Method to invoke when the SaveAllResults command is executed.
        /// </summary>
        private void OnSaveAllResultsExecute()
        {
            // TODO: Remember last picked folder (in user settings), and start from there
            if (_selectDirectoryService.DetermineDirectory())
            {
                var folderName = _selectDirectoryService.DirectoryName;

                try
                {
                    //foreach (var plotTabViewModel in Tabs.Where(t => t is PlotTabViewModel).Cast<PlotTabViewModel>())
                    //{
                    //    Benchmark.Benchmarker.ExportResultsToPdf(plotTabViewModel.PlotModel, plotTabViewModel.Result, folderName);
                    //    Benchmark.Benchmarker.ExportResultsToCsv(plotTabViewModel.Result, folderName);
                    //}
                }
                catch (Exception ex)
                {
                    _messageService.ShowError(string.Format(UIStrings.MainViewModel_SaveAllResultsAction_Error_saving_results_));

                    Log.Error(ex);
                }
            }
        }
        #endregion

        #region Methods
        protected override async Task Initialize()
        {
            await base.Initialize();

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

        protected override async Task Close()
        {
            _uiServiceHost.Stop();

            _uiServiceHost.Ping -= OnPing;
            _uiServiceHost.UpdateResult -= OnUpdateResult;

            await base.Close();
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