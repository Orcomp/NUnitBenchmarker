namespace NUnitBenchmarker.Core.Infrastructure.DependencyInjection
{
	/// <summary>
	///     Very simple Inversion of Control class for global DI resolving.
	///     NOTE: This class is DI container independent
	/// </summary>
	public class Dependency
	{
		#region Constants and Fields

		private static IResolver resolver;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Initializes the specified resolver.
		/// </summary>
		/// <param name="resolver">The resolver.</param>
		public static void Initialize(IResolver resolver)
		{
			Dependency.resolver = resolver;
		}

		/// <summary>
		///     Use this for all service / interface resolving
		/// </summary>
		/// <typeparam name="T">Interface to resolve</typeparam>
		/// <returns>Resolved concrete instance (service)</returns>
		public static T Resolve<T>()
		{
			return resolver.Resolve<T>();
		}

		#endregion
	}
}