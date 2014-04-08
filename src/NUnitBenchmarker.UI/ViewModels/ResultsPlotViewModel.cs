// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsPlotViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.UIService.Data;
    using OxyPlot;

    public class ResultsPlotViewModel : ViewModelBase
    {
        public ResultsPlotViewModel(BenchmarkResult benchmarkResult)
        {
            Argument.IsNotNull(() => benchmarkResult);

            BenchmarkResult = benchmarkResult;

            UpdateModel();
        }

        #region Properties
        public BenchmarkResult BenchmarkResult { get; private set; }

        public PlotModel PlotModel { get; set; }
        #endregion

        #region Methods
        private void UpdateModel()
        {
            
        }
        #endregion
    }
}