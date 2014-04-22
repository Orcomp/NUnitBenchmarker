// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsDataViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System;
    using System.Data;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker;
    using Data;

    public class ResultsDataViewModel : ViewModelBase
    {
        public ResultsDataViewModel(BenchmarkResult benchmarkResult)
        {
            Argument.IsNotNull(() => benchmarkResult);

            Title = benchmarkResult.Key;
            BenchmarkResult = benchmarkResult;

            UpdateResults(BenchmarkResult);
        }

        #region Properties
        public BenchmarkResult BenchmarkResult { get; private set; }

        public DataTable DataTable { get; private set; }
        #endregion

        #region Methods
        private void UpdateResults(BenchmarkResult result)
        {
            if (!string.Equals(result.Key, BenchmarkResult.Key))
            {
                return;
            }

            DataTable = new BenchmarkFinalTabularData(result).DataTable;
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