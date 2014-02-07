using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.UI.Properties;
using NUnitBenchmarker.UI.Resources;
using NUnitBenchmarker.UIService;

namespace NUnitBenchmarker.UI.ViewModel
{
	/// <summary>
	///     Class MainViewModel: MVVM ViewModel for MainWindow
	/// </summary>
	public class MainViewModel : ViewModelBase, IDisposable
	{
		private bool alwaysOnTop;
		private string lastPingMessage;
		private int mainWindowHeight;
		private int mainWindowLeft;
		private WindowState mainWindowState;
		private int mainWindowTop;
		private int mainWindowWidth;
		private Rect restoreBounds;
		private string statusBarText;
		private ICommand exitMenuItemClickCommand;
		private ICommand openMenuItemClickCommand;
		private string lastChoosenAssembly;
		private ObservableCollection<AssemblyListItem> assemblyListItemsSource;
		private readonly ILogger logger;
		private IViewService viewService;
		private ICommand removeMenuItemClickCommand;


		/// <summary>
		///     Initializes a new instance of the <see cref="MainViewModel" /> class.
		/// </summary>
		public MainViewModel(ILogger logger, IViewService viewService)
			: this((IMessenger)null)
		{
			assemblyListItemsSource = new ObservableCollection<AssemblyListItem>();
			this.logger = logger;
			this.viewService = viewService;
		}

		/// <summary>
		///     Initializes a new instance of the ViewModelBase class.
		/// </summary>
		/// <param name="messenger">
		///     An instance of a <see cref="T:GalaSoft.MvvmLight.Messaging.Messenger" />
		///     used to broadcast messages to other objects. If null, this class
		///     will attempt to broadcast using the Messenger's default
		///     instance.
		/// </param>
		public MainViewModel(IMessenger messenger)
			: base(messenger)
		{
			var serviceHost = Dependency.Resolve<IUIServiceHost>();
			try
			{
				serviceHost.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			serviceHost.Ping += OnPing;
			serviceHost.GetAssemblyNames += GetAssemblyNames;
			RestoreMainWindow();
		}

		private IEnumerable<string> GetAssemblyNames()
		{
			return AssemblyListItemsSource.Where(item => item.IsChecked).Select(item => item.Path);
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the last ping message.
		/// </summary>
		/// <value>The last ping message.</value>
		public string LastPingMessage
		{
			get { return lastPingMessage; }
			set
			{
				if (lastPingMessage == value)
				{
					return;
				}

				lastPingMessage = value;
				RaisePropertyChanged(() => LastPingMessage);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the status bar text to display.
		/// </summary>
		/// <value>The status bar text to display.</value>
		public string StatusBarText
		{
			get { return statusBarText; }
			set
			{
				statusBarText = value ?? string.Empty;
				RaisePropertyChanged(() => StatusBarText);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets the window title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return string.Format("NUnit Benchmarker v{0}", Assembly.GetExecutingAssembly().GetName().Version); }
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the main window top.
		/// </summary>
		/// <value>The main window top.</value>
		public int MainWindowTop
		{
			get { return mainWindowTop; }
			set
			{
				mainWindowTop = value;
				RaisePropertyChanged(() => MainWindowTop);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the main window left.
		/// </summary>
		/// <value>The main window left.</value>
		public int MainWindowLeft
		{
			get { return mainWindowLeft; }
			set
			{
				mainWindowLeft = value;
				RaisePropertyChanged(() => MainWindowLeft);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the width of the main window.
		/// </summary>
		/// <value>The width of the main window.</value>
		public int MainWindowWidth
		{
			get { return mainWindowWidth; }
			set
			{
				mainWindowWidth = value;
				RaisePropertyChanged(() => MainWindowWidth);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the height of the main window.
		/// </summary>
		/// <value>The height of the main window.</value>
		public int MainWindowHeight
		{
			get { return mainWindowHeight; }
			set
			{
				mainWindowHeight = value;
				RaisePropertyChanged(() => MainWindowHeight);
			}
		}


		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the state of the main window.
		/// </summary>
		/// <value>The state of the main window.</value>
		public WindowState MainWindowState
		{
			get { return mainWindowState; }
			set
			{
				mainWindowState = value;
				RaisePropertyChanged(() => MainWindowState);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the restore bounds.
		/// </summary>
		/// <value>The restore bounds.</value>
		public Rect RestoreBounds
		{
			get { return restoreBounds; }
			set { restoreBounds = value; }
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets a value indicating whether the 
		///		NUnitBenchmarker main window is always on top or not.
		/// </summary>
		/// <value><c>true</c> if [always on top]; otherwise, <c>false</c>.</value>
		public bool AlwaysOnTop
		{
			get { return alwaysOnTop; }
			set
			{
				alwaysOnTop = value;
				RaisePropertyChanged(() => AlwaysOnTop);
			}
		}


		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			SaveMainWindow();
			var serviceHost = Dependency.Resolve<IUIServiceHost>();
			serviceHost.Stop();
		}

		/// <summary>
		///     Called when Ping event raises (by the UIService)
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>System.String.</returns>
		private string OnPing(string message)
		{
			LastPingMessage = message;
			StatusBarText = string.Format("Incoming Ping message: {0}", message);
			return string.Format("Welcome to the machine: {0}", message);
		}
		/// <summary>
		///     Gets the exit menu item click command for MVVM binding.
		/// </summary>
		/// <value>The exit menu item click command.</value>
		public ICommand ExitMenuItemClickCommand
		{
			get { return exitMenuItemClickCommand ?? (exitMenuItemClickCommand = new RelayCommand<object>(ExitAction)); }
		}

		/// <summary>
		///     Exit Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void ExitAction(object dummy)
		{
			Application.Current.Shutdown(0);
		}


		/// <summary>
		///     Gets the open menu item click command for MVVM binding.
		/// </summary>
		/// <value>The open menu item click command.</value>
		public ICommand OpenMenuItemClickCommand
		{
			get { return openMenuItemClickCommand ?? (openMenuItemClickCommand = new RelayCommand<object>(OpenAction)); }
		}


		/// <summary>
		///     Open Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void OpenAction(object dummy)
		{
			string fileName;
			var result = viewService.ShowOpenFile(out fileName);
			if (result != null && result.Value)
			{
				try
				{
					if (AssemblyListItemsSource.Any( item => item.Path == fileName))
					{
						viewService.ShowMessage(string.Format(UIStrings.Message_assembly_is_already_in_the_list, fileName));
						return;
					}
					var assembly = Assembly.LoadFile(fileName);
					var fullName = assembly.FullName.Replace(',', '\n');

					AssemblyListItemsSource.Add(new AssemblyListItem(this)
					{
						Path = fileName,
						ShortName = Path.GetFileName(fileName),
						IsChecked = true,
						FullName = string.Format("{0}\nPath: ({1})", fullName, fileName) 
					});
				}
				catch (Exception e)
				{
					viewService.ShowMessage(string.Format(UIStrings.Message_error_loading_assembly, fileName));
					logger.Error(e);
				}
			}
		}


		/// <summary>
		///     Restores the main window position an size
		/// </summary>
		private void RestoreMainWindow()
		{
			MainWindowTop = Settings.Default.MainWindowTop;
			MainWindowLeft = Settings.Default.MainWindowLeft;
			MainWindowHeight = Settings.Default.MainWindowHeight;
			MainWindowWidth = Settings.Default.MainWindowWidth;
			if (Settings.Default.MainWindowMaximized)
			{
				MainWindowState = WindowState.Maximized;
			}
			else
			{
				MainWindowState = WindowState.Normal;
			}
		}

		/// <summary>
		///     Saves the main window position and size.
		/// </summary>
		private void SaveMainWindow()
		{
			if (MainWindowState == WindowState.Maximized)
			{
				// Using RestoreBounds as the current values will be 0, 0 and the size of the screen.
				// As RestoreBounds is a read only property getting (binding) RestoreBounds done via data piping.
				Settings.Default.MainWindowTop = (int) RestoreBounds.Top;
				Settings.Default.MainWindowLeft = (int) RestoreBounds.Left;
				Settings.Default.MainWindowHeight = (int) RestoreBounds.Height;
				Settings.Default.MainWindowWidth = (int) RestoreBounds.Width;
				Settings.Default.MainWindowMaximized = true;
			}
			else
			{
				Settings.Default.MainWindowTop = MainWindowTop;
				Settings.Default.MainWindowLeft = MainWindowLeft;
				Settings.Default.MainWindowHeight = MainWindowHeight;
				Settings.Default.MainWindowWidth = MainWindowWidth;
				Settings.Default.MainWindowMaximized = false;
			}

			Settings.Default.Save();
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the LastChoosenAssembly display.
		/// </summary>
		/// <value>The title.</value>
		public string LastChoosenAssembly
		{
			get { return lastChoosenAssembly; }
			set
			{
				lastChoosenAssembly = value;
				RaisePropertyChanged(() => LastChoosenAssembly);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the the multiselect list of the loaded assemblies.
		/// </summary>
		/// <value>The title.</value>
		public ObservableCollection<AssemblyListItem> AssemblyListItemsSource
		{
			get
			{
				return assemblyListItemsSource;
			}
			set
			{
				assemblyListItemsSource = value;
				RaisePropertyChanged(() => AssemblyListItemsSource);
			}
		}

		/// <summary>
		///     AssemblyListItem Remove Action event handler.
		/// </summary>
		/// <param name="item">Item to remove</param>
		internal void RemoveAction(AssemblyListItem item)
		{
			AssemblyListItemsSource.Remove(item);
			//viewService.ShowMessage(string.Format("TODO: Implement"));
		}

	}
}