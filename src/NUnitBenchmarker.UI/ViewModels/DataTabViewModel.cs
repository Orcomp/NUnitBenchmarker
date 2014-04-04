using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using NUnitBenchmarker.Benchmark;
using NUnitBenchmarker.UIService.Data;

namespace NUnitBenchmarker.UI.ViewModels
{
	public class DataTabViewModel : TabViewModel
	{
		private string dataTitle; // Backing field for property dataTitle
		private BenchmarkResult result;


		public DataTabViewModel(string key, string dataTitle, MainViewModel mainViewModel) : base(key, mainViewModel)
		{
			this.dataTitle = dataTitle;
			Title = string.Format("{0} data", dataTitle); 
		}

		/// <summary>
		/// Observable property for MVVM. Gets or sets state dataTitle. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public string DataTitle
		{
			get { return dataTitle; }

			set
			{
				if (dataTitle == value)
				{
					return;
				}
				dataTitle = value;
				RaisePropertyChanged(() => DataTitle);
			}
		}


		public void UpdateResults(BenchmarkResult result)
		{
			this.result = result;
			DataTable = new BenchmarkFinalTabularData(result).DataTable;
		}

		private DataTable dataTable; // Backing field for property DataTable

		/// <summary>
		/// Observable property for MVVM. Gets or sets DataTable which is the datasource
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public DataTable DataTable
		{
			get { return dataTable; }

			set
			{
				if (dataTable == value)
				{
					return;
				}
				dataTable = value;
				RaisePropertyChanged(() => DataTable);
			}
		}
	}
}