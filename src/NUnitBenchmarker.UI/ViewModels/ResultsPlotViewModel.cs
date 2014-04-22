// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsPlotViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Linq;
    using Catel;
    using Catel.MVVM;
    using Data;
    using Models;
    using Services;
    using OxyPlot;

    public class ResultsPlotViewModel : ViewModelBase
    {
        private readonly ISettings _settings;

        public ResultsPlotViewModel(BenchmarkResult benchmarkResult, ISettings settings)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => settings);

            BenchmarkResult = benchmarkResult;
            _settings = settings;

            UpdateResults(BenchmarkResult);
        }

        #region Properties
        public BenchmarkResult BenchmarkResult { get; private set; }

        public PlotModel PlotModel { get; private set; }
        #endregion

        #region Methods
        private void UpdateResults(BenchmarkResult result)
        {
            if (!string.Equals(result.Key, BenchmarkResult.Key))
            {
                return;
            }

            int dummy;
            PlotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
                ? Benchmarker.CreatePlotModel(result, !_settings.IsLogarithmicTimeAxisChecked)
                : Benchmarker.CreateCategoryPlotModel(result, !_settings.IsLogarithmicTimeAxisChecked);
        }

        protected override void Initialize()
        {
            base.Initialize();

            BenchmarkResult.Updated += OnBenchmarkUpdated;
        }

        protected override void Close()
        {
            BenchmarkResult.Updated -= OnBenchmarkUpdated;

            base.Close();
        }

        private void OnBenchmarkUpdated(object sender, EventArgs e)
        {
            UpdateResults(BenchmarkResult);
        }
        #endregion
    }
}