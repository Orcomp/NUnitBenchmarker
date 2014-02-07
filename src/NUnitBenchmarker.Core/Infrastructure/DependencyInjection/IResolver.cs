namespace NUnitBenchmarker.Core.Infrastructure.DependencyInjection
{
	/// <summary>
	///     Very simple Inversion of Control interface for global DI resolving.
	///     NOTE: This interface is DI container independent.
	/// </summary>
	public interface IResolver
	{
		#region Public Methods and Operators

		/// <summary>
		///     Use this for all service / interface resolving
		/// </summary>
		/// <typeparam name="T">Interface to resolve</typeparam>
		/// <returns>Resolved concrete instance (service)</returns>
		T Resolve<T>();

		#endregion
	}
}