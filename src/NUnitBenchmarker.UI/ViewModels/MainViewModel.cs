// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using NUnitBenchmarker.Data;
    using NUnitBenchmarker.Models;
    using NUnitBenchmarker.Resources;
    using NUnitBenchmarker.Services;

    /// <summary>
    /// Class MainViewModel: MVVM ViewModel for MainWindow
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region Fields
        private readonly IViewService _viewService;
        private readonly IUIServiceHost _uiServiceHost;
        private readonly IMessageService _messageService;
        private readonly ISelectDirectoryService _selectDirectoryService;
        private readonly IOpenFileService _openFileService;
        private readonly IReflectionService _reflectionService;
        private readonly IPleaseWaitService _pleaseWaitService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel(IViewService viewService, IUIServiceHost uiServiceHost, IMessageService messageService,
            ISelectDirectoryService selectDirectoryService, IOpenFileService openFileService, ICommandManager commandManager,
            ISettings settings, IReflectionService reflectionService, IPleaseWaitService pleaseWaitService)
        {
            Argument.IsNotNull(() => viewService);
            Argument.IsNotNull(() => uiServiceHost);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => selectDirectoryService);
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => commandManager);
            Argument.IsNotNull(() => settings);
            Argument.IsNotNull(() => reflectionService);
            Argument.IsNotNull(() => pleaseWaitService);

            _viewService = viewService;
            _uiServiceHost = uiServiceHost;
            _messageService = messageService;
            _selectDirectoryService = selectDirectoryService;
            _openFileService = openFileService;
            _reflectionService = reflectionService;
            _pleaseWaitService = pleaseWaitService;
            Settings = settings;

            Roots = new ObservableCollection<ReflectionNodeViewModel>();
            BenchmarkResults = new ObservableCollection<BenchmarkResult>();

            FileOpen = new Command(OnFileOpenExecute);
            SwitchTimeAxis = new Command(OnSwitchTimeAxisExecute);
            SaveAllResults = new Command(OnSaveAllResultsExecute);

            commandManager.RegisterCommand("File.Open", FileOpen, this);
            commandManager.RegisterCommand("File.SaveAllResults", SaveAllResults, this);
        }
        #endregion

        #region Properties
        public override string Title
        {
            get { return string.Format("NUnit Benchmarker v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
        }

        public string LastPingMessage { get; set; }

        public ISettings Settings { get; private set; }

        public ObservableCollection<ReflectionNodeViewModel> Roots { get; private set; }

        public ObservableCollection<BenchmarkResult> BenchmarkResults { get; private set; }

        public BenchmarkResult SelectedBenchmarkResult { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets the FileOpen command.
        /// </summary>
        public Command FileOpen { get; private set; }

        /// <summary>
        /// Method to invoke when the FileOpen command is executed.
        /// </summary>
        private void OnFileOpenExecute()
        {
            _openFileService.IsMultiSelect = false;
            _openFileService.Filter = "Assembly Files (.dll, .exe)|*.dll;*.exe";
            if (_openFileService.DetermineFile())
            {
                string fileName = _openFileService.FileName;

                try
                {
                    if (Roots.Any(item => (item.Data).Path == fileName))
                    {
                        _messageService.ShowInformation(string.Format(UIStrings.Message_assembly_is_already_in_the_list, fileName));
                        return;
                    }

                    _pleaseWaitService.Show(() =>
                    {
                        var assemblyEntry = _reflectionService.GetAssemblyEntry(fileName);
                        var nodeViewModel = new ReflectionNodeViewModel(assemblyEntry, null);

                        nodeViewModel.RequestRemove += OnNodeViewModelOnRequestRemove;
                        Roots.Add(nodeViewModel);
                    });
                }
                catch (Exception ex)
                {
                    _messageService.ShowError(string.Format(UIStrings.Message_error_loading_assembly, fileName));

                    Log.Error(ex);
                }
            }
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
                    _viewService.ShowMessage(string.Format(UIStrings.MainViewModel_SaveAllResultsAction_Error_saving_results_));

                    Log.Error(ex);
                }
            }
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            try
            {
                _uiServiceHost.Start();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            _uiServiceHost.Ping += OnPing;
            _uiServiceHost.GetImplementations += OnGetImplementations;
            _uiServiceHost.UpdateResult += OnUpdateResult;
        }

        protected override void Close()
        {
            _uiServiceHost.Stop();

            _uiServiceHost.Ping -= OnPing;
            _uiServiceHost.GetImplementations -= OnGetImplementations;
            _uiServiceHost.UpdateResult -= OnUpdateResult;

            base.Close();
        }

        private string OnPing(string message)
        {
            LastPingMessage = message;

            return string.Format("Welcome to the machine: {0}", message);
        }

        private IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification typeSpecification)
        {
            var result = new List<TypeSpecification>();
            foreach (var node in Roots)
            {
                result.AddRange(node.GetChildrenData()
                    .Where(e => e.LeafEntry)
                    .Select(e => new TypeSpecification
                    {
                        AssemblyPath = e.Path,
                        FullName = ((TypeEntry)e).TypeFullName
                    }));
            }

            return result;
        }

        private void OnUpdateResult(BenchmarkResult result)
        {
            // TODO: Update existing result instead of refreshing complete view

            bool reselect = false;
            int index = BenchmarkResults.Count;

            var currentItem = (from x in BenchmarkResults
                               where string.Equals(x.Key, result.Key)
                               select x).FirstOrDefault();
            if (currentItem != null)
            {
                reselect = ReferenceEquals(SelectedBenchmarkResult, currentItem);
                index = BenchmarkResults.IndexOf(currentItem);
                BenchmarkResults.Remove(currentItem);
            }

            BenchmarkResults.Insert(index, result);

            if (reselect || SelectedBenchmarkResult == null)
            {
                SelectedBenchmarkResult = result;
            }
        }

        private void OnNodeViewModelOnRequestRemove(object sender, EventArgs eventArgs)
        {
            var node = sender as ReflectionNodeViewModel;
            if (node == null)
            {
                return;
            }

            node.RequestRemove -= OnNodeViewModelOnRequestRemove;
            Roots.Remove(node);
        }
        #endregion
    }
}