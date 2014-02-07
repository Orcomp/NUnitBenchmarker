using System.Collections.Generic;
using System.ServiceModel;

namespace NUnitBenchmarker.UIService
{
	[ServiceContract]
	public interface IUIService
	{
		/// <summary>
		///     Sent by the client to get diagnostic ping.
		/// </summary>
		/// <param name="message">The message.</param>
		[OperationContract]
		string Ping(string message);

		/// <summary>
		/// Gets choosen assembly names.
		/// </summary>
		/// <returns>IEnumerable{System.String}.</returns>
		[OperationContract]
		IEnumerable<string> GetAssemblyNames();
	}
}