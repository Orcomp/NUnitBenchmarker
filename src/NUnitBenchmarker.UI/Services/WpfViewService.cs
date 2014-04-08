// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WpfViewService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.Services
{
    using System;
    using System.Windows.Forms;
    using Catel;
    using Catel.Services;
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

    /// <summary>
    /// IViewService interface implementation for WPF.
    /// </summary>
    internal class WpfViewService : IViewService
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly IMessageService _messageService;

        #region Constructors
        public WpfViewService(IDispatcherService dispatcherService, IMessageService messageService)
        {
            Argument.IsNotNull(() => dispatcherService);
            Argument.IsNotNull(() => messageService);

            _dispatcherService = dispatcherService;
            _messageService = messageService;
        }
        #endregion

        #region IViewService Members
        /// <summary>
        ///     Shows a message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessage(string message)
        {
            _dispatcherService.Invoke(() => _messageService.Show(message));
        }

        public bool ShowQuestion(string message)
        {
            var result = MessageResult.No;

            _dispatcherService.Invoke(() => result = _messageService.Show(message, "Question", MessageButton.YesNo, MessageImage.Question));

            return result == MessageResult.Yes;
        }

        ///// <summary>
        ///// Shows the Open File dialog
        ///// </summary>
        ///// <param name="fileName">The picked filename (user input)</param>
        ///// <returns><c>true</c> if a file was choosen, <c>false</c> otherwise.</returns>
        //public bool? ShowOpenFile(out string fileName)
        //{
        //    bool? result = false;
        //    OpenFileDialog dialog = null;
        //    _dispatcher.Invoke(new Action(() =>
        //    {
        //        // Set filter for file extension and default file extension
        //        dialog = new OpenFileDialog
        //        {
        //            DefaultExt = ".dll",
        //            Filter = "Assembly Files (.dll, .exe)|*.dll;*.exe"
        //        };

        //        // Display OpenFileDialog by calling ShowDialog method
        //        result = dialog.ShowDialog(GetActiveWindow());
        //    }));

        //    fileName = dialog.FileName;
        //    return result;
        //}
        #endregion
    }
}