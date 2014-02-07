using System.Diagnostics;

namespace NUnitBenchmarker.Core.Infrastructure.Logging
{
	/// <summary>
	///     Simple log listener implementation
	/// </summary>
	public class LogListener : ILogListener
	{
		#region Public Methods and Operators

		/// <summary>
		///     Writes a line to the log target.
		/// </summary>
		/// <param name="format">The format string like in string.Format.</param>
		/// <param name="args">The variable length args array line in string.Format.</param>
		public void WriteLine(string format, params object[] args)
		{
			Debug.WriteLine(format, args);
		}

		#endregion
	}
}