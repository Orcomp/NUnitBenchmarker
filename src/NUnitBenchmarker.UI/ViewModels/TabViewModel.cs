using System;
using System.ComponentModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NUnitBenchmarker.UI.ViewModels
{
	/// <summary>
	/// This ViewModelBase subclass requests to be removed 
	/// from the UI when its CloseCommand executes.
	/// This class is abstract.
	/// </summary>
	public abstract class TabViewModel : ViewModelBase
	{
		private string key; // Backing field for property Key
		protected MainViewModel mainViewModel;
		ICommand closeCommand;

		protected TabViewModel(string key, MainViewModel mainViewModel)
		{
			this.key = key;
			this.mainViewModel = mainViewModel;
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
		/// Returns the command that, when invoked, attempts
		/// to remove this tab from the user interface.
		/// </summary>
		public ICommand CloseCommand
		{
			get
			{
				return closeCommand ?? (closeCommand = new RelayCommand(OnRequestClose));
			}
		}

		/// <summary>
		/// Raised when this tab should be removed from the UI.
		/// </summary>
		public event EventHandler RequestClose;

		private void OnRequestClose()
		{
			EventHandler handler = RequestClose;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
				
		}

		private string title; // Backing field for property Title

		/// <summary>
		/// Observable property for MVVM. Gets or sets Title for the tab. 
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
		/// Observable property for MVVM. Gets or sets state TabsSelectedIndex. 
		/// Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface 
		/// </summary>
		/// <value>The property value. If the new value is the same as the current property value
		/// then no PropertyChange event is raised.
		/// </value>
		public int TabControlSelectedIndex
		{
			get
			{
				return mainViewModel.TabsSelectedIndex;
			}

			set
			{

				//if (mainViewModel.TabsSelectedIndex == value)
				//{
				//	return;
				//}
				mainViewModel.TabsSelectedIndex = value;
				//RaisePropertyChanged(() => TabControlSelectedIndex);
			}
		}

	}
}