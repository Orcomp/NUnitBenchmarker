using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;
using NUnitBenchmarker.Core.Infrastructure.Logging;

namespace NUnitBenchmarker.UIService
{
	/// <summary>
	///     Class UIService
	///     Service definition class for exchanging data with Runner client
	/// </summary>
	public class UIService : IUIService
	{
		/// <summary>
		///     Sent by the client to get diagnostic ping.
		/// </summary>
		/// <param name="message">The message.</param>
		public string Ping(string message)
		{
			LogCall(new {message});
			
			var host = Dependency.Resolve<IUIServiceHost>();
			return host.OnPing(message);			
		}

		/// <summary>
		/// Gets choosen assembly names.
		/// </summary>
		/// <returns>IEnumerable{System.String}.</returns>
		public IEnumerable<string> GetAssemblyNames()
		{
			LogCall(null);
			var host = Dependency.Resolve<IUIServiceHost>();
			return host.OnGetAssemblyNames();			
		}

		private void LogCall( object parameters, [CallerMemberName] string memberName = "" )
		{
			Dependency.Resolve<ILogger>().Info("UIService command '{0}' was received with the following parameters: {1}.", memberName, AnonymousToString(parameters));
		}

		private string AnonymousToString(object @object)
		{
			if (@object == null)
			{
				return string.Empty;
			}
			string result = @object.GetType().GetProperties().Aggregate(
				string.Empty, 
				(current, propertyInfo) 
					=> 
				current + string.Format("'{0}': >{1}<, ", propertyInfo.Name, propertyInfo.GetValue(@object)));

			return result.Trim().Trim(',');
		}
	}
}