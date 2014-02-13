using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace NUnitBenchmarker.UI.ViewModels
{
	/// <summary>
	/// Class AssemblyListItem. Represents an item in the loaded assembly list
	/// </summary>
	public class AssemblyListItem : ObservableObject
	{
		private string fullName;
		private bool isChecked;
		private string shortName;
		private ICommand removeMenuItemClickCommand;
		private readonly MainViewModel mainViewModel;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyListItem"/> class.
		/// </summary>
		/// <param name="mainViewModel">The main view model (used to relaying the Remove command to the model what holsd the whole list</param>
		public AssemblyListItem(MainViewModel mainViewModel)
		{
			this.mainViewModel = mainViewModel;
		}

		/// <summary>
		/// Gets or sets the short name, which is the display text in the UI
		/// </summary>
		/// <value>The short name.</value>
		public string ShortName
		{
			get { return shortName; }
			set
			{
				shortName = value;
				RaisePropertyChanged(() => ShortName);
			}
		}

		/// <summary>
		/// Gets or sets the full name of the assembly. The full name is used to give the additional metainfo for the user
		/// </summary>
		/// <value>The full name.</value>
		public string FullName
		{
			get { return fullName; }
			set
			{
				fullName = value;
				RaisePropertyChanged(() => FullName);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this item is checked by the user.
		/// </summary>
		/// <value><c>true</c> if this item is checked; otherwise, <c>false</c>.</value>
		public bool IsChecked
		{
			get { return isChecked; }
			set
			{
				isChecked = value;
				RaisePropertyChanged(() => IsChecked);
			}
		}


		/// <summary>
		/// Gets or sets the full original loading path.
		/// </summary>
		/// <value>The path.</value>
		public string Path { get; set; }

		/// <summary>
		/// Gets the remove menu item click command for MVVM binding.
		/// </summary>
		/// <value>The remove menu item click command.</value>
		public ICommand RemoveMenuItemClickCommand
		{
			get { return removeMenuItemClickCommand ?? (removeMenuItemClickCommand = new RelayCommand<object>(RemoveAction)); }
		}


		/// <summary>
		/// AssemblyListItem Remove Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void RemoveAction(object dummy)
		{
			mainViewModel.RemoveAction(this);
		}
	}
}