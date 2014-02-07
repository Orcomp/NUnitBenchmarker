#region usings

using System;

#endregion

namespace NUnitBenchmarker.Core.Infrastructure.Logging
{
	/// <summary>
	///     Interface ILogger for Dependency Injection logging implementations
	/// </summary>
	public interface ILogger
	{
		void Info(string message, params object[] args);

		void Warn(string message, params object[] args);

		void Debug(string message, params object[] args);

		void Error(string message, params object[] args);

		void Error(Exception e);

		void Error(Exception e, string message, params object[] args);

		void Fatal(string message, params object[] args);

		void Fatal(Exception e);

		void Fatal(Exception e, string message, params object[] args);
	}

}