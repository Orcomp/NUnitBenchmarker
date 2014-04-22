﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsPlotViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System.Linq;
    using Catel;
    using Catel.MVVM;
    using Data;
    using Models;
    using Services;
    using OxyPlot;

    public class ResultsPlotViewModel : ViewModelBase
    {
        private readonly IUIServiceHost _uiServiceHost;
        private readonly ISettings _settings;

        public ResultsPlotViewModel(BenchmarkResult benchmarkResult, IUIServiceHost uiServiceHost, ISettings settings)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => uiServiceHost);
            Argument.IsNotNull(() => settings);

            BenchmarkResult = benchmarkResult;
            _uiServiceHost = uiServiceHost;
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

            _uiServiceHost.UpdateResult += UpdateResults;
        }

        protected override void Close()
        {
            _uiServiceHost.UpdateResult -= UpdateResults;

            base.Close();
        }
        #endregion
    }
}