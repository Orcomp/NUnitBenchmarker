// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsDataViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using System.Data;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.UIService.Data;

    public class ResultsDataViewModel : ViewModelBase
    {
        public ResultsDataViewModel(BenchmarkResult benchmarkResult)
        {
            Argument.IsNotNull(() => benchmarkResult);

            BenchmarkResult = benchmarkResult;

            UpdateData();
        }

        #region Properties
        public BenchmarkResult BenchmarkResult { get; private set; }

        public DataTable DataTable { get; set; }
        #endregion

        #region Methods
        private void UpdateData()
        {

        }
        #endregion
    }
}