using System.ComponentModel;
using GalaSoft.MvvmLight;

namespace NUnitBenchmarker.UI.ViewModels
{
	public class TabItemViewModel : ViewModelBase
	{
		private MainViewModel mainViewModel;
		private string name; 
		private string title;

		public TabItemViewModel(MainViewModel mainViewModel, string name)
		{
			this.mainViewModel = mainViewModel;
			this.name = name;
		}

		/// <summary>
		/// Observable property for MVVM. Gets or sets state Title. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public string Title
		{
			get { return title; }

			set
			{
				if (title == value)
				{
					return;
				}
				title = value;
				RaisePropertyChanged(() => Title);
			}
		}

		/// <summary>
		/// Observable property for MVVM. Gets or sets the name. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public string Name
		{
			get { return name; }

			set
			{
				if (name == value)
				{
					return;	
				}
				name = value;
				RaisePropertyChanged(() => Name);
			}
		}
	}
}