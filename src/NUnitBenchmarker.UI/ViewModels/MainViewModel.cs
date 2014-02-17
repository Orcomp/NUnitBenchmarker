using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net.Core;
using NUnitBenchmarker.Benchmark;
using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;
using NUnitBenchmarker.Core.Infrastructure.Logging.Log4Net;
using NUnitBenchmarker.UI.Properties;
using NUnitBenchmarker.UI.Resources;
using NUnitBenchmarker.UIService;
using NUnitBenchmarker.UIService.Data;
using ILogger = NUnitBenchmarker.Core.Infrastructure.Logging.ILogger;

namespace NUnitBenchmarker.UI.ViewModels
{
	/// <summary>
	///     Class MainViewModel: MVVM ViewModel for MainWindow
	/// </summary>
	public class MainViewModel : ViewModelBase, IDisposable
	{
		private readonly PublisherAppender appender;
		private readonly ILogger logger;
		private readonly IViewService viewService;
		private bool alwaysOnTop;
		private ObservableCollection<AssemblyListItem> assemblyListItemsSource;
		private ICommand exitMenuItemClickCommand;
		private string lastPingMessage;
		private ObservableCollection<LogItemViewModel> logItems; // Backing field for property LogItems
		private int mainWindowHeight;
		private int mainWindowLeft;
		private WindowState mainWindowState;
		private int mainWindowTop;
		private int mainWindowWidth;
		private ICommand openMenuItemClickCommand;
		private Rect restoreBounds;
		private string splitHeightBottom;
		private string splitHeightTop;
		private string splitWidthLeft;
		private string splitWidthRight;
		private string statusBarText;
		private ObservableCollection<TabViewModel> tabs;


		/// <summary>
		///     Initializes a new instance of the <see cref="MainViewModel" /> class.
		/// </summary>
		public MainViewModel(ILogger logger, IViewService viewService, PublisherAppender appender)
		{
			assemblyListItemsSource = new ObservableCollection<AssemblyListItem>();
			this.logger = logger;
			this.viewService = viewService;
			this.appender = appender;

			var serviceHost = Dependency.Resolve<IUIServiceHost>();
			try
			{
				serviceHost.Start();
			}
			catch (Exception e)
			{
				logger.Error(e);
			}

			serviceHost.Ping += OnPing;
			serviceHost.GetAssemblyNames += GetAssemblyNames;
			serviceHost.UpdateResult += UpdateResults;

			appender.LoggingEventAppended += LoggingEventAppended;

			LogItems = new ObservableCollection<LogItemViewModel>();
			RestoreMainWindow();
			//var dataTabViewModel = new DataTabViewModel("test1", "test2");
			//dataTabViewModel.DataTable = CreateDemoTable();
			//Tabs.Add(dataTabViewModel);


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
		///     NUnitBenchmarker main window is always on top or not.
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
		///     Observable property for MVVM binding. Gets or sets a value of the vertical splitter position
		/// </summary>
		/// <value><c>true</c> if [always on top]; otherwise, <c>false</c>.</value>
		public string SplitHeightTop
		{
			get { return splitHeightTop; }
			set
			{
				splitHeightTop = value;
				RaisePropertyChanged(() => SplitHeightTop);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets a value of the vertical splitter position
		/// </summary>
		/// <value><c>true</c> if [always on top]; otherwise, <c>false</c>.</value>
		public string SplitHeightBottom
		{
			get { return splitHeightBottom; }
			set
			{
				splitHeightBottom = value;
				RaisePropertyChanged(() => SplitHeightBottom);
			}
		}


		/// <summary>
		///     Observable property for MVVM binding. Gets or sets a value of the horizontal splitter position
		/// </summary>
		/// <value><c>true</c> if [always on top]; otherwise, <c>false</c>.</value>
		public string SplitWidthLeft
		{
			get { return splitWidthLeft; }
			set
			{
				splitWidthLeft = value;
				RaisePropertyChanged(() => SplitWidthLeft);
			}
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets a value of the horizontal splitter position
		/// </summary>
		/// <value><c>true</c> if [always on top]; otherwise, <c>false</c>.</value>
		public string SplitWidthRight
		{
			get { return splitWidthRight; }
			set
			{
				splitWidthRight = value;
				RaisePropertyChanged(() => SplitWidthRight);
			}
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
		///     Gets the open menu item click command for MVVM binding.
		/// </summary>
		/// <value>The open menu item click command.</value>
		public ICommand OpenMenuItemClickCommand
		{
			get { return openMenuItemClickCommand ?? (openMenuItemClickCommand = new RelayCommand<object>(OpenAction)); }
		}

		/// <summary>
		///     Observable property for MVVM binding. Gets or sets the the multiselect list of the loaded assemblies.
		/// </summary>
		/// <value>The title.</value>
		public ObservableCollection<AssemblyListItem> AssemblyListItemsSource
		{
			get { return assemblyListItemsSource; }
			set
			{
				assemblyListItemsSource = value;
				RaisePropertyChanged(() => AssemblyListItemsSource);
			}
		}

		/// <summary>
		///     Returns the collection of available tabs to display.
		///     A 'tab' is a ViewModel that can request to be closed.
		/// </summary>
		public ObservableCollection<TabViewModel> Tabs
		{
			get
			{
				if (tabs == null)
				{
					tabs = new ObservableCollection<TabViewModel>();
					tabs.CollectionChanged += OnTabsChanged;
				}
				return tabs;
			}
		}

		/// <summary>
		///     Observable property for MVVM. Gets or sets state LogItems.
		///     Set accessor raises PropertyChanged event on <see cref="INotifyPropertyChanged" /> interface
		/// </summary>
		/// <value>
		///     The property value. If the new value is the same as the current property value
		///     then no PropertyChange event is raised.
		/// </value>
		public ObservableCollection<LogItemViewModel> LogItems
		{
			get { return logItems; }

			set
			{
				if (logItems == value)
				{
					return;
				}
				logItems = value;
				RaisePropertyChanged(() => LogItems);
			}
		}

		public ICommand SwitchTimeAxis
		{
			get { throw new NotImplementedException(); }
		}


		/// <summary>
		///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			SaveMainWindow();
			var serviceHost = Dependency.Resolve<IUIServiceHost>();
			serviceHost.Stop();
			serviceHost.Ping -= OnPing;
			serviceHost.GetAssemblyNames -= GetAssemblyNames;
			appender.LoggingEventAppended -= LoggingEventAppended;
		}

		private void LoggingEventAppended(LoggingEvent @event, string renderedMessage)
		{
			// log4net's @event.RenderedMessage does not seem to use the renderers despite its name
			LogItems.Add(new LogItemViewModel(@event.TimeStamp.ToString(), @event.Level.ToString(), @event.RenderedMessage));
		}

		private IEnumerable<string> GetAssemblyNames()
		{
			return AssemblyListItemsSource.Where(item => item.IsChecked).Select(item => item.Path);
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
		///     Exit Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void ExitAction(object dummy)
		{
			Application.Current.Shutdown(0);
		}


		/// <summary>
		///     Open Action event handler.
		/// </summary>
		/// <param name="dummy">not used here</param>
		private void OpenAction(object dummy)
		{
			string fileName;
			bool? result = viewService.ShowOpenFile(out fileName);
			if (result != null && result.Value)
			{
				try
				{
					if (AssemblyListItemsSource.Any(item => item.Path == fileName))
					{
						viewService.ShowMessage(string.Format(UIStrings.Message_assembly_is_already_in_the_list, fileName));
						return;
					}
					Assembly assembly = Assembly.LoadFile(fileName);
					string fullName = assembly.FullName.Replace(", ", "\n");

					AssemblyListItemsSource.Add(new AssemblyListItem(this)
					{
						Path = fileName,
						ShortName = Path.GetFileName(fileName),
						IsChecked = true,
						FullName = string.Format("{0}\nLoaded from: {1}", fullName, fileName)
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

			SplitHeightTop = Settings.Default.SplitHeightTop;
			SplitHeightBottom = Settings.Default.SplitHeightBottom;
			SplitWidthLeft = Settings.Default.SplitWidthLeft;
			SplitWidthRight = Settings.Default.SplitWidthRight;
			AlwaysOnTop = Settings.Default.AlwaysOnTop;
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

			Settings.Default.SplitHeightTop = SplitHeightTop;
			Settings.Default.SplitHeightBottom = SplitHeightBottom;
			Settings.Default.SplitWidthLeft = SplitWidthLeft;
			Settings.Default.SplitWidthRight = SplitWidthRight;
			Settings.Default.AlwaysOnTop = AlwaysOnTop;

			var loadedAssemblies = new StringCollection();
			loadedAssemblies.AddRange(GetAssemblyNames().ToArray());
			Settings.Default.LoadedAssemblies = loadedAssemblies;

			Settings.Default.Save();
		}

		/// <summary>
		///     AssemblyListItem Remove Action event handler.
		/// </summary>
		/// <param name="item">Item to remove</param>
		internal void RemoveAction(AssemblyListItem item)
		{
			AssemblyListItemsSource.Remove(item);
		}

		private void OnTabsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null && e.NewItems.Count != 0)
			{
				foreach (TabViewModel tabViewModel in e.NewItems)
				{
					tabViewModel.RequestClose += OnTabRequestClose;
				}
			}

			if (e.OldItems != null && e.OldItems.Count != 0)
			{
				foreach (TabViewModel tabViewModel in e.OldItems)
				{
					tabViewModel.RequestClose -= OnTabRequestClose;
				}
			}
		}

		private void OnTabRequestClose(object sender, EventArgs e)
		{
			Tabs.Remove((TabViewModel) sender);
		}

		/// <summary>
		///     Event handler for client's UpdateResult message.
		///		Routes the message to the addressed PlotTabViewModel instance,
		///		and creates it if does not exist yet
		/// </summary>
		/// <param name="result">Benchmark results coming from the client</param>
		private void UpdateResults(BenchmarkResult result)
		{
			var model = GetPlotTabViewModel(result.Key, true);
			ActivateTab<PlotTabViewModel>(model.Key);
			model.UpdateResults(result);

			if (!result.IsLast)
			{
				return;
			}
			var dataTabViewModel = GetDataTabViewModel(result.Key, true);
			dataTabViewModel.UpdateResults(result);
			ActivateTab<DataTabViewModel>(dataTabViewModel.Key);
		}

		private void ActivateTab<T>(string key)
		{
			TabViewModel found = Tabs.FirstOrDefault(pt => pt is T && pt.Key == key);
			if (found != null)
			{
				SetActiveTab(found);
			}
		}

		private void SetActiveTab(TabViewModel tabViewModel)
		{
			ICollectionView collectionView = CollectionViewSource.GetDefaultView(Tabs);
			if (collectionView != null)
			{
				collectionView.MoveCurrentTo(tabViewModel);
			}
		}

		private PlotTabViewModel GetPlotTabViewModel(string key, bool create)
		{
			var found =
				(PlotTabViewModel) Tabs.Where(t => t is PlotTabViewModel).FirstOrDefault(pt => ((PlotTabViewModel) pt).Key == key);

			if (found == null && create)
			{
				found = new PlotTabViewModel(key, key);
				Tabs.Add(found);
			}
			return found;
		}

		private DataTabViewModel GetDataTabViewModel(string key, bool create)
		{
			var found =
				(DataTabViewModel)Tabs.Where(t => t is DataTabViewModel).FirstOrDefault(pt => ((DataTabViewModel)pt).Key == key);

			if (found == null && create)
			{
				found = new DataTabViewModel(key, key);
				Tabs.Add(found);
			}
			return found;
		}


		private ICommand switchTimeAxisCommand;

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
			// TODO: Implement
		}


		private static DataTable CreateDemoTable()
		{
			var table = new DataTable("Demo");

			int colummnCount = 5;
			int rowCount = 5;
			for(int i = 1; i<= colummnCount; i++)
			{
				table.Columns.Add(string.Format("Column{0}", i), typeof(double));
			}

			for (int r = 1; r <= rowCount; r++)
			{
				var row = table.NewRow();
				table.Rows.Add(row);
				for (int i = 1; i <= colummnCount; i++)
				{
					row[string.Format("Column{0}", i)] = i * r;
				}
			}
			return table;
		}













		





	}
}