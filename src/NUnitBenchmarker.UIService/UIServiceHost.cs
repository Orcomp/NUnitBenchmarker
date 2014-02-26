using System;
using System.Collections.Generic;
using System.ServiceModel;
using NUnitBenchmarker.UIService.Data;

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


		public event Func<TypeSpecification, IEnumerable<TypeSpecification>> GetImplementations;
		public IEnumerable<TypeSpecification> OnGetImplementations(TypeSpecification interfaceType)
		{
			// Prevent race condition if other thread accidentally unsubscribes
			var handler = GetImplementations;
			// Call the handler if any:
			if (handler != null)
			{
				return handler(interfaceType);
			}
			return null;
		}


		public event Action<BenchmarkResult> UpdateResult;
		public void OnUpdateResult(BenchmarkResult result)
		{
			// Prevent race condition if other thread accidentally unsubscribes
			var handler = UpdateResult;
			// Call the handler if any:
			if (handler != null)
			{
				handler(result);
			}
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