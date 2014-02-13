using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace NUnitBenchmarker.UI.ViewModels
{
	public class PlotTabViewModel : TabViewModel
	{
		RelayCommand demoCommand;

		private string key; // Backing field for property Key
		private string plotTitle; // Backing field for property PlotTitle

		public PlotTabViewModel(string key, string plotTitle)
		{
			this.key = key;
			this.plotTitle = plotTitle;
			Title = plotTitle; // This is the inherited Tab Title
			PlotModel = new PlotModel(plotTitle);
			SetUpModel();
		}

		/// <summary>
		/// Observable property for MVVM. Gets or sets state Key. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public string Key
		{
			get { return key; }

			set
			{
				if (key == value)
				{
					return;
				}
				key = value;
				RaisePropertyChanged(() => Key);
			}
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

		/// <summary>
		/// Returns a demo command
		/// </summary>
		public ICommand DemoCommand
		{
			get
			{
				if (demoCommand == null)
				{
					demoCommand = new RelayCommand(Demo, () => true);
				}
				return demoCommand;
			}
		}

		public void Demo()
		{
			Debug.WriteLine("Demo");
		}

		public PlotModel SetUpModel()
		{
			var plotModel = new PlotModel("sss");
			plotModel.LegendTitle = "Legend";
			plotModel.LegendOrientation = LegendOrientation.Horizontal;
			plotModel.LegendPlacement = LegendPlacement.Outside;
			plotModel.LegendPosition = LegendPosition.TopRight;
			plotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
			plotModel.LegendBorder = OxyColors.Black;

			var dateAxis = new DateTimeAxis(AxisPosition.Bottom, "Date", "dd/MM/yy HH:mm") { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, IntervalLength = 80 };
			plotModel.Axes.Add(dateAxis);
			var valueAxis = new LinearAxis(AxisPosition.Left, 0) { MajorGridlineStyle = LineStyle.Solid, MinorGridlineStyle = LineStyle.Dot, Title = "Value" };
			plotModel.Axes.Add(valueAxis);
			return plotModel;
		}

		private void LoadData()
		{
			//List<Measurement> measurements = Data.GetData();

			//var dataPerDetector = measurements.GroupBy(m => m.DetectorId).ToList();

			//foreach (var data in dataPerDetector)
			//{
			//	var lineSerie = new LineSeries
			//	{
			//		StrokeThickness = 2,
			//		MarkerSize = 3,
			//		MarkerStroke = colors[data.Key],
			//		MarkerType = markerTypes[data.Key],
			//		CanTrackerInterpolatePoints = false,
			//		Title = string.Format("Detector {0}", data.Key),
			//		Smooth = false,
			//	};

			//	data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
			//	PlotModel.Series.Add(lineSerie);
			//}
		}


		private readonly List<OxyColor> colors = new List<OxyColor>
                                            {
                                                OxyColors.Green,
                                                OxyColors.IndianRed,
                                                OxyColors.Coral,
                                                OxyColors.Chartreuse,
                                                OxyColors.Azure
                                            };

		private readonly List<MarkerType> markerTypes = new List<MarkerType>
                                                   {
                                                       MarkerType.Plus,
                                                       MarkerType.Star,
                                                       MarkerType.Diamond,
                                                       MarkerType.Triangle,
                                                       MarkerType.Cross
                                                   };



	}
}