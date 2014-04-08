// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultsViewModel.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker.UI.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using NUnitBenchmarker.UIService.Data;

    public class ResultsViewModel : ViewModelBase
    {
        public ResultsViewModel(BenchmarkResult benchmarkResult)
        {
            Argument.IsNotNull(() => benchmarkResult);

            BenchmarkResult = benchmarkResult;
        }

        public BenchmarkResult BenchmarkResult { get; private set; }
    }
}