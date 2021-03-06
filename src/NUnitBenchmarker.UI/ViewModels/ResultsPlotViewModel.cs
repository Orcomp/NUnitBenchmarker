﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsPlotViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Catel;
    using Catel.MVVM;
    using Data;
    using Models;
    using OxyPlot.Series;
    using Services;
    using OxyPlot;
    using System.Threading.Tasks;

    public class ResultsPlotViewModel : ViewModelBase
    {
        public ResultsPlotViewModel(BenchmarkResult benchmarkResult, ISettings settings)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => settings);

            BenchmarkResult = benchmarkResult;
            IsLogarithmicTimeAxisChecked = settings.IsLogarithmicTimeAxisChecked;
        }

        #region Properties
        public BenchmarkResult BenchmarkResult { get; private set; }

        public PlotModel PlotModel { get; private set; }
        #endregion

        #region Methods
        private void UpdateResults(BenchmarkResult result = null)
        {
            if (result == null)
            {
                result = BenchmarkResult;
            }

            if (!string.Equals(result.Key, BenchmarkResult.Key))
            {
                return;
            }

            PlotModel = PlotFactory.CreatePlotModel(result, !IsLogarithmicTimeAxisChecked);
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            BenchmarkResult.Updated += OnBenchmarkUpdated;

            UpdateResults(BenchmarkResult);
        }

        protected override async Task CloseAsync()
        {
            BenchmarkResult.Updated -= OnBenchmarkUpdated;

            await base.CloseAsync();
        }

        private void OnBenchmarkUpdated(object sender, EventArgs e)
        {
            UpdateResults(BenchmarkResult);
        }

        public bool IsLogarithmicTimeAxisChecked { get; set; }

        private ICommand isLogarithmicTimeAxisCommand;

        /// <summary>
        ///     Gets the 'IsLogarithmicTimeAxis' command for MVVM binding.
        /// </summary>
        /// <value>The 'IsLogarithmicTimeAxis' command.</value>
        public ICommand IsLogarithmicTimeAxisCommand
        {
            get { return isLogarithmicTimeAxisCommand ?? (isLogarithmicTimeAxisCommand = new Command<object>(IsLogarithmicTimeAxisCommandAction, IsLogarithmicTimeAxisCommandCanExecute)); }
        }

        /// <summary>
        ///     'IsLogarithmicTimeAxis' command event handler.
        /// </summary>
        /// <param name="arg">The optional command argument.</param>
        private void IsLogarithmicTimeAxisCommandAction(object arg)
        {
            // Binding path does not work (Settings.IsLogarithmicTimeAxisChecked) so bound directly
            UpdateResults();
        }

        /// <summary>
        ///     'IsLogarithmicTimeAxis' command event handler.
        /// </summary>
        /// <param name="arg">The optional command argument.</param>
        private bool IsLogarithmicTimeAxisCommandCanExecute(object arg)
        {
            return true;
        }
        #endregion
    }
}