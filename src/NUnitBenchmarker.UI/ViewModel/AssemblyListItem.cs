using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NUnitBenchmarker.UI.ViewModel
{
	public class AssemblyListItem : ObservableObject
	{
		private string fullName;
		private bool isChecked;
		private string shortName;
		private ICommand removeMenuItemClickCommand;
		private MainViewModel mainViewModel;

		public AssemblyListItem(MainViewModel mainViewModel)
		{
			this.mainViewModel = mainViewModel;
		}

		public string ShortName
		{
			get { return shortName; }
			set
			{
				shortName = value;
				RaisePropertyChanged(() => ShortName);
			}
		}
		
		public string FullName
		{
			get { return fullName; }
			set
			{
				fullName = value;
				RaisePropertyChanged(() => FullName);
			}
		}

		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				isChecked = value;
				RaisePropertyChanged(() => IsChecked);
			}
		}


		public string Path { get; set; }

		/// <summary>
		///     Gets the remove menu item click command for MVVM binding.
		/// </summary>
		/// <value>The remove menu item click command.</value>
		public ICommand RemoveMenuItemClickCommand
		{
			get { return removeMenuItemClickCommand ?? (removeMenuItemClickCommand = new RelayCommand<object>(RemoveAction)); }
		}


		/// <summary>
		///     AssemblyListItem Remove Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void RemoveAction(object dummy)
		{
			mainViewModel.RemoveAction(this);
		}

	}
}