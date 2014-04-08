// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlotTabViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using System.ComponentModel;
    using System.Linq;
    using Catel.MVVM;
    using NUnitBenchmarker.Benchmark;
    using NUnitBenchmarker.UIService.Data;
    using OxyPlot;

    public class PlotTabViewModel : ViewModelBase
    {
        #region Fields
        #endregion

        #region Constructors
        public PlotTabViewModel()
        {
            PlotModel = new PlotModel();
        }
        #endregion

        #region Properties
        public PlotModel PlotModel { get; private set; }

        public bool IsLogarithmicTimeAxisChecked { get; set; }

        public BenchmarkResult Result { get; set; }
        #endregion

        #region Methods
        private void UpdateResults(BenchmarkResult result)
        {
            int dummy;
            PlotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
                ? Benchmarker.CreatePlotModel(result, !IsLogarithmicTimeAxisChecked)
                : Benchmarker.CreateCategoryPlotModel(result, !IsLogarithmicTimeAxisChecked);
        }
        #endregion
    }
}