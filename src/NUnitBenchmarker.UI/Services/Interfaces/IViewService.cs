// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IViewService.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.Services
{
    /// <summary>
    /// Interface IViewService: Interface for MVVM message showing.
    /// </summary>
    public interface IViewService
    {
        #region Methods
        /// <summary>
        ///     Shows a message.
        /// </summary>
        /// <param name="message">The message.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Shows a question
        /// </summary>
        /// <param name="message">The message as question.</param>
        /// <returns><c>true</c> if user clicked Yes, <c>false</c> otherwise</returns>
        bool ShowQuestion(string message);
        #endregion
    }
}