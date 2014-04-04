using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Fasterflect;
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

		public PlotTabViewModel(string key, string plotTitle, MainViewModel mainViewModel)
			: base(key, mainViewModel)
		{
			this.plotTitle = plotTitle;
			Title = string.Format("{0} graph", plotTitle); 
			PlotModel = new PlotModel(plotTitle);
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

		private BenchmarkResult result;


		private bool isLogarithmicTimeAxisChecked;

		private void UpdateResults(BenchmarkResult result)
		{
			int dummy;
			PlotModel = int.TryParse(result.TestCases.FirstOrDefault(), out dummy)
				? Benchmarker.CreatePlotModel(result, !isLogarithmicTimeAxisChecked)
				: Benchmarker.CreateCategoryPlotModel(result, !isLogarithmicTimeAxisChecked);
		}

		
		
		public bool IsLogarithmicTimeAxisChecked
		{
			get { return isLogarithmicTimeAxisChecked; }
			set
			{
				if (value != isLogarithmicTimeAxisChecked)
				{
					isLogarithmicTimeAxisChecked = value;
					if (Result != null)
					{
						UpdateResults(Result);
					}
				}
				RaisePropertyChanged(()=>IsLogarithmicTimeAxisChecked);
				mainViewModel.RaiseIsLogarithmicTimeAxisCheckedChanged();
			}
		}

		public BenchmarkResult Result
		{
			get { return result; }
			set
			{
				result = value;
				UpdateResults(result);
			}
		}
	}
}