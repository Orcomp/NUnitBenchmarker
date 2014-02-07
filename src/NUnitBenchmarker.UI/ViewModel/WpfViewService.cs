using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace NUnitBenchmarker.UI.ViewModel
{
	/// <summary>
	///     IViewService interface implementation for WPF
	/// </summary>
	internal class WpfViewService : IViewService
	{
		#region Constants and Fields

		private readonly Dispatcher dispatcher;

		#endregion

		#region Constructors and Destructors

		public WpfViewService()
		{
			dispatcher = Dispatcher.CurrentDispatcher;
		}

		#endregion

		#region Public Methods and Operators

		public Dispatcher GetDispatcher()
		{
			return dispatcher;
		}

		/// <summary>
		///     Shows a message.
		/// </summary>
		/// <param name="message">The message.</param>
		public void ShowMessage(string message)
		{
			dispatcher.Invoke(new Action(() => MessageBox.Show(GetActiveWindow(), message)));
		}

		public bool ShowQuestion(string message)
		{
			if (dispatcher.Thread.ManagedThreadId != Thread.CurrentThread.ManagedThreadId)
			{
				var @event = new ManualResetEvent(false);
				var result = MessageBoxResult.No;
				dispatcher.Invoke(() =>
									  {
										  result = MessageBox.Show(GetActiveWindow(), message, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
										  @event.Set();
									  });
				if (@event.WaitOne(1000))
				{
					return result == MessageBoxResult.Yes;
				}
				ShowMessage("Error in synchronizing ShowQuestion");
				return false;
			}
			return MessageBoxResult.Yes == MessageBox.Show(GetActiveWindow(), message, "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
		}


		/// <summary>
		/// Shows the Open File dialog
		/// </summary>
		/// <param name="fileName">The picked filename (user input)</param>
		/// <returns><c>true</c> if a file was choosen, <c>false</c> otherwise.</returns>
		public bool? ShowOpenFile(out string fileName)
		{
			bool? result = false;
			OpenFileDialog dialog = null;
			dispatcher.Invoke(() =>
								  {
									  // Set filter for file extension and default file extension
									  dialog = new OpenFileDialog
									  {
										  DefaultExt = ".dll", 
										  Filter = "Assembly Files (.dll, .exe)|*.dll;*.exe"
									  };

									  // Display OpenFileDialog by calling ShowDialog method
									  result = dialog.ShowDialog(GetActiveWindow());
								  });
								  
			fileName = dialog.FileName;
			return result;
		}

		public void Marshall(Action<object> action, object arg)
		{
			dispatcher.Invoke(action, DispatcherPriority.Normal, arg);
		}

		#endregion

		private Window GetActiveWindow()
		{
			Window result;
			try
			{
				result = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive)
						 ?? Application.Current.MainWindow;
			}
			catch (Exception)
			{
				result = Application.Current.MainWindow;
			}
			return result;
		}
	}
}