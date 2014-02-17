using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NUnitBenchmarker.Benchmark;
using NUnitBenchmarker.UIService.Data;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace NUnitBenchmarker.UI.ViewModels
{
	public class PlotTabViewModel : TabViewModel
	{
		private string plotTitle; // Backing field for property PlotTitle

		public PlotTabViewModel(string key, string plotTitle) : base(key)
		{
			this.plotTitle = plotTitle;
			Title = string.Format("{0} graph", plotTitle); 
			PlotModel = new PlotModel(plotTitle);
			isLinear = true;
		}

		/// <summary>
		/// Observable property for MVVM. Gets or sets state PlotTitle. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public string PlotTitle
		{
			get { return plotTitle; }

			set
			{
				if (plotTitle == value)
				{
					return;
				}
				plotTitle = value;
				RaisePropertyChanged(() => PlotTitle);
			}
		}

		private PlotModel plotModel; // Backing field for property PlotModel

		/// <summary>
		/// Observable property for MVVM. Gets or sets state PlotModel. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public PlotModel PlotModel
		{
			get { return plotModel; }

			set
			{
				if (plotModel == value)
				{
					return;
				}
				plotModel = value;
				RaisePropertyChanged(() => PlotModel);
			}
		}

		private ICommand switchTimeAxisCommand;
		private BenchmarkResult result;
		private bool isLinear;

		/// <summary>
		///     Gets the SwitchTimeAxis command for MVVM binding.
		/// </summary>
		/// <value>The SwitchTimeAxis command.</value>
		public ICommand SwitchTimeAxisCommand
		{
			get { return switchTimeAxisCommand ?? (switchTimeAxisCommand = new RelayCommand<object>(SwitchTimeAxisAction)); }
		}

		/// <summary>
		///     SwitchTimeAxis event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void SwitchTimeAxisAction(object dummy)
		{
			isLinear = !isLinear;
			if (result != null)
			{
				UpdateResults(result);
			}
		}

		public void UpdateResults(BenchmarkResult result)
		{
			this.result = result;
			int dummy;
			PlotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
				? Benchmarker.CreatePlotModel(result, isLinear)
				: Benchmarker.CreateCategoryPlotModel(result, isLinear);
		}
	}
}