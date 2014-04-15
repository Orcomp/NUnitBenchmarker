// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypesViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Catel;
    using Catel.Logging;
    using Catel.MVVM;
    using Catel.Services;
    using NUnitBenchmarker.Data;
    using NUnitBenchmarker.Models;
    using NUnitBenchmarker.Resources;
    using NUnitBenchmarker.Services;

    public class TypesViewModel : ViewModelBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IOpenFileService _openFileService;
        private readonly IMessageService _messageService;
        private readonly IPleaseWaitService _pleaseWaitService;
        private readonly IUIServiceHost _uiServiceHost;
        private readonly IReflectionService _reflectionService;

        public TypesViewModel(IOpenFileService openFileService, IMessageService messageService, IPleaseWaitService pleaseWaitService,
            IUIServiceHost uiServiceHost, IReflectionService reflectionService, ICommandManager commandManager)
        {
            Argument.IsNotNull(() => openFileService);
            Argument.IsNotNull(() => messageService);
            Argument.IsNotNull(() => pleaseWaitService);
            Argument.IsNotNull(() => uiServiceHost);
            Argument.IsNotNull(() => reflectionService);
            Argument.IsNotNull(() => commandManager);

            _openFileService = openFileService;
            _messageService = messageService;
            _pleaseWaitService = pleaseWaitService;
            _uiServiceHost = uiServiceHost;
            _reflectionService = reflectionService;

            Assemblies = new ObservableCollection<AssemblyEntry>();

            FileOpen = new Command(OnFileOpenExecute);

            commandManager.RegisterCommand("File.Open", FileOpen, this);
        }

        #region Properties
        public ObservableCollection<AssemblyEntry> Assemblies { get; private set; }
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
                    if (Assemblies.Any(item => item.Path == fileName))
                    {
                        _messageService.ShowInformation(string.Format(UIStrings.Message_assembly_is_already_in_the_list, fileName));
                        return;
                    }

                    _pleaseWaitService.Show(() =>
                    {
                        var assemblyEntry = _reflectionService.GetAssemblyEntry(fileName);
                        Assemblies.Add(assemblyEntry);
                    });
                }
                catch (Exception ex)
                {
                    _messageService.ShowError(string.Format(UIStrings.Message_error_loading_assembly, fileName));

                    Log.Error(ex);
                }
            }
        }
        #endregion

        #region Methods
        protected override void Initialize()
        {
            base.Initialize();

            _uiServiceHost.GetImplementations += OnGetImplementations;
        }

        protected override void Close()
        {
            _uiServiceHost.GetImplementations -= OnGetImplementations;

            base.Close();
        }

        private IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification typeSpecification)
        {
            Argument.IsNotNull(() => typeSpecification);

            var result = new List<TypeSpecification>();
            foreach (var node in Assemblies)
            {
                result.AddRange(node.Children
                    .Where(e => e.LeafEntry)
                    .Select(e => new TypeSpecification
                    {
                        AssemblyPath = e.Path,
                        FullName = ((TypeEntry)e).TypeFullName
                    }));
            }

            return result;
        }
        #endregion
    }
}