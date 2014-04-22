// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using Data;
    using Models;
    using Resources;
    using Services;

    public class TypesViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IOpenFileService _openFileService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IUIServiceHost _uiServiceHost;
        private readonly IReflectionService _reflectionService;

        private readonly List<AssemblyEntry> _assemblies = new List<AssemblyEntry>();

        public TypesViewModel(IOpenFileService openFileService, IPleaseWaitService pleaseWaitService,
            IUIServiceHost uiServiceHost, IReflectionService reflectionService, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => pleaseWaitService);
            Argument.IsNotNull(() => uiServiceHost);
            Argument.IsNotNull(() => reflectionService);
            Argument.IsNotNull(() => commandManager);

            _openFileService = openFileService;
            _pleaseWaitService = pleaseWaitService;
            _uiServiceHost = uiServiceHost;
            _reflectionService = reflectionService;

            Assemblies = new List<AssemblyEntry>();

            FileOpen = new Command(OnFileOpenExecute);
            ClearFilter = new Command(OnClearFilterExecute);

            commandManager.RegisterCommand("File.Open", FileOpen, this);
        }

        #region Properties
        public List<AssemblyEntry> Assemblies { get; private set; }

        public string Filter { get; set; }
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
                AddAssembly(_openFileService.FileName, true, true);
            }
        }

        /// <summary>
        /// Gets the ClearFilter command.
        /// </summary>
        public Command ClearFilter { get; private set; }

        /// <summary>
        /// Method to invoke when the ClearFilter command is executed.
        /// </summary>
        private void OnClearFilterExecute()
        {
            Filter = string.Empty;
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            _uiServiceHost.GetImplementations += OnGetImplementations;
            _uiServiceHost.UpdateResult += OnUpdateResult;
        }

        protected override void Close()
        {
            _uiServiceHost.GetImplementations -= OnGetImplementations;
            _uiServiceHost.UpdateResult -= OnUpdateResult;

            base.Close();
        }

        private void OnFilterChanged()
        {
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            var assemblies = new List<AssemblyEntry>();
            var filter = Filter;

            foreach (var assembly in _assemblies)
            {
                if (assembly.ApplyFilter(filter))
                {
                    assemblies.Add(assembly);
                }
            }

            Assemblies = assemblies;
        }

        private IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification typeSpecification)
        {
            Argument.IsNotNull(() => typeSpecification);

            var result = new List<TypeSpecification>();
            foreach (var node in Assemblies)
            {
                result.AddRange(node.Children
                    .Select(e => new TypeSpecification
                    {
                        AssemblyPath = e.Path,
                        FullName = ((TypeEntry)e).TypeFullName
                    }));
            }

            return result;
        }

        private void OnUpdateResult(BenchmarkResult benchmark)
        {
            var typeSpecification = benchmark.TypeSpecification;
            if (typeSpecification == null)
            {
                Log.Warning("Benchmark.TypeSpecification is null, cannot dynamically load assembly information");
                return;
            }

            AddAssembly(typeSpecification.AssemblyPath);

            SelectSpecificTypes(typeSpecification.FullName);
        }

        private void AddAssembly(string assemblyFileName, bool usePleaseWaitService = false, bool defaultIsChecked = false)
        {
            Argument.IsNotNull(() => assemblyFileName);

            try
            {
                var assembly = Assemblies.FirstOrDefault(item => string.Equals(item.Path, assemblyFileName));
                if (assembly != null)
                {
                    Log.Info(UIStrings.Message_assembly_is_already_in_the_list, assemblyFileName);
                    return;
                }

                Log.Info("Loading assembly from '{0}'", assemblyFileName);

                Action action = () =>
                {
                    var assemblyEntry = _reflectionService.GetAssemblyEntry(assemblyFileName, defaultIsChecked);
                    assembly = assemblyEntry;

                    _assemblies.Add(assemblyEntry);

                    UpdateFilter();
                };

                if (usePleaseWaitService)
                {
                    _pleaseWaitService.Show();
                }
                else
                {
                    action();
                }
            }
            catch (Exception)
            {
                Log.Error(UIStrings.Message_error_loading_assembly, assemblyFileName);
            }
        }

        private void SelectSpecificTypes(params string[] selectedTypes)
        {
            foreach (var assembly in Assemblies)
            {
                assembly.SelectTypes(selectedTypes);
            }
        }
        #endregion
    }
}