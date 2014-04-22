﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsDataViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.ViewModels
{
    using System.Data;
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker;
    using Data;
    using Services;

    public class ResultsDataViewModel : ViewModelBase
    {
        private readonly IUIServiceHost _uiServiceHost;

        public ResultsDataViewModel(BenchmarkResult benchmarkResult, IUIServiceHost uiServiceHost)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => uiServiceHost);

            Title = benchmarkResult.Key;
            BenchmarkResult = benchmarkResult;
            _uiServiceHost = uiServiceHost;

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