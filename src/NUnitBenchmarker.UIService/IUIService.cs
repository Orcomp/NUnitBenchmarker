using System.Collections.Generic;
using System.ServiceModel;
using log4net.Core;
using NUnitBenchmarker.UIService.Data;

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
		/// Gets the implementations to test
		/// </summary>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>IEnumerable{TypeSpecification}.</returns>
		[OperationContract]
		IEnumerable<TypeSpecification> GetImplementations(TypeSpecification interfaceType);


		/// <summary>
		/// Logs a standard log4net logging event
		/// </summary>
		[OperationContract]
		void Log(string loggingEventString);

		/// <summary>
		/// Updates the result in the UI
		/// </summary>
		[OperationContract]
		void UpdateResult(BenchmarkResult result);
	}
}