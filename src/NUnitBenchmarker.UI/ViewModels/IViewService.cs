namespace NUnitBenchmarker.UI.ViewModels
{
	/// <summary>
	///     Interface IViewService: Interface for MVVM message showing
	/// </summary>
	public interface IViewService
	{
		/// <summary>
		///     Shows the Open File dialog
		/// </summary>
		/// <param name="fileName">The picked filename (user input)</param>
		bool? ShowOpenFile(out string fileName);

		/// <summary>
		///     Shows a message.
		/// </summary>
		/// <param name="message">The message.</param>
		void ShowMessage(string message);

		/// <summary>
		/// Shows a question
		/// </summary>
		/// <param name="message">The message as question.</param>
		/// <returns><c>true</c> if user clicked Yes, <c>false</c> otherwise</returns>
		bool ShowQuestion(string message);
	}
}