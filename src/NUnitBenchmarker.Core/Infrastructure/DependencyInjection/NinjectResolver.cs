using Ninject;

namespace NUnitBenchmarker.Core.Infrastructure.DependencyInjection
{
	/// <summary>
	///     Very simple Inversion of Control class for global DI resolving.
	///     NOTE: This class is the Ninject specific implementation of the IResolver interface
	/// </summary>
	public class NinjectResolver : IResolver
	{
		#region Constants and Fields

		private readonly StandardKernel kernel;

		#endregion

		#region Constructors and Destructors

		/// <summary>
		///     Initializes a new instance of the <see cref="NinjectResolver" /> class.
		/// </summary>
		/// <param name="kernel">The Ninject kernel.</param>
		public NinjectResolver(StandardKernel kernel)
		{
			this.kernel = kernel;
		}

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Use this for all service / interface resolving
		/// </summary>
		/// <typeparam name="T">Interface to resolve</typeparam>
		/// <returns>Resolved concrete instance (service)</returns>
		public T Resolve<T>()
		{
			return kernel.Get<T>();
		}

		#endregion
	}
}