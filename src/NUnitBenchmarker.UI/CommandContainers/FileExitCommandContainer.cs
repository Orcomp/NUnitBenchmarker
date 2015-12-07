// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationExitCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Threading.Tasks;
    using Catel;
    using Catel.MVVM;
    using Catel.Services;

    public class FileExitCommandContainer : CommandContainerBase
    {
        #region Fields
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructors
        public FileExitCommandContainer(ICommandManager commandManager, INavigationService navigationService)
            : base(Commands.File.Exit, commandManager)
        {
            Argument.IsNotNull(() => navigationService);

            _navigationService = navigationService;
        }
        #endregion

        #region Methods
        protected override async Task ExecuteAsync(object parameter)
        {
            _navigationService.CloseApplication();
        }
        #endregion
    }
}