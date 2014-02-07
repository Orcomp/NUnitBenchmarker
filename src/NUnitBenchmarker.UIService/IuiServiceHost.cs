using System;
using System.Collections.Generic;

namespace NUnitBenchmarker.UIService
{
	public interface IUIServiceHost
	{
		event Func<string, string> Ping;
		string OnPing(string message);

		event Func<IEnumerable<string>> GetAssemblyNames;
		IEnumerable<string> OnGetAssemblyNames();


		/// <summary>
		///     Starts the Runner - UI communication service.
		/// </summary>
		void Start();

		/// <summary>
		///     Stops the Runner - UI communication service.
		/// </summary>
		void Stop();
	}
}