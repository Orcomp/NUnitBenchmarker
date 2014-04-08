// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTabViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using System.ComponentModel;
    using System.Data;
    using Catel.MVVM;
    using NUnitBenchmarker.Benchmark;
    using NUnitBenchmarker.UIService.Data;

    public class DataTabViewModel : ViewModelBase
    {
        #region Fields
        #endregion

        #region Constructors
        public DataTabViewModel(string dataTitle)
        {
            DataTitle = dataTitle;
            Title = string.Format("{0} data", dataTitle);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Observable property for MVVM. Gets or sets state dataTitle. 
        /// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
        /// </summary>
        /// <value>The property value. If the new value is the same as the current property value
        /// then no PropertyChange event is raised.
        /// </value>
        public string DataTitle { get; set; }

        /// <summary>
        /// Observable property for MVVM. Gets or sets DataTable which is the datasource
        /// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
        /// </summary>
        /// <value>The property value. If the new value is the same as the current property value
        /// then no PropertyChange event is raised.
        /// </value>
        public DataTable DataTable { get; set; }
        #endregion

        #region Methods
        public void UpdateResults(BenchmarkResult result)
        {
            //Result = result;
            DataTable = new BenchmarkFinalTabularData(result).DataTable;
        }
        #endregion
    }
}