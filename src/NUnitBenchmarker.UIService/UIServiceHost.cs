using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace NUnitBenchmarker.UIService
{
	public class UIServiceHost : IUIServiceHost
	{
		private ServiceHost host;

		public event Func<string, string> Ping;
		public string OnPing(string message)
		{
			// Prevent race condition if other thread accidentally unsubscribes
			var handler = Ping;
			// Call the handler if any:
			if (handler != null)
			{
				return handler(message);
			}
			return null;

		}

		public event Func<IEnumerable<string>> GetAssemblyNames;
		public IEnumerable<string> OnGetAssemblyNames()
		{
			// Prevent race condition if other thread accidentally unsubscribes
			var handler = GetAssemblyNames;
			// Call the handler if any:
			if (handler != null)
			{
				return handler();
			}
			return null;
		}

		/// <summary>
		///     Starts the Runner - UI communication service.
		/// </summary>
		public void Start()
		{
			host = new ServiceHost(typeof(UIService));
			host.Open();
		}

		/// <summary>
		///     Stops the Runner - UI communication service.
		/// </summary>
		public void Stop()
		{
			host.Close();
		}

	}

}