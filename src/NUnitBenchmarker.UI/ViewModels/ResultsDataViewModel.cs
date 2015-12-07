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
    using System.Threading.Tasks;

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

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            BenchmarkResult.Updated += OnBenchmarkUpdated;
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
        #endregion
    }
}