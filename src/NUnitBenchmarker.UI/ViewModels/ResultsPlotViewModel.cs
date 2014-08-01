// --------------------------------------------------------------------------------------------------------------------
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

    public class ResultsPlotViewModel : ViewModelBase
    {
        public ResultsPlotViewModel(BenchmarkResult benchmarkResult, ISettings settings)
        {
            Argument.IsNotNull(() => benchmarkResult);
            Argument.IsNotNull(() => settings);

            BenchmarkResult = benchmarkResult;
	        IsLogarithmicTimeAxisChecked = settings.IsLogarithmicTimeAxisChecked;
            UpdateResults(BenchmarkResult);
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

            int dummy;
            PlotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
                ? Benchmarker.CreatePlotModel(result, !IsLogarithmicTimeAxisChecked)
                : Benchmarker.CreateCategoryPlotModel(result, !IsLogarithmicTimeAxisChecked);
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

		//private void IsLogarithmicTimeAxisCheckedChanged(object sender, PropertyChangedEventArgs e)
		//{
		//	if (e.PropertyName == "IsLogarithmicTimeAxisChecked")
		//	{
		//		UpdateResults(BenchmarkResult);				
		//	}
		//}


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