using Ninject;
using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;
using NUnitBenchmarker.Core.Infrastructure.Logging;
using NUnitBenchmarker.UI.ViewModel;
using NUnitBenchmarker.UIService;

namespace NUnitBenchmarker.UI
{
	/// <summary>
	///     Dependency Injection initializer class
	/// </summary>
	public class BootStrapper
	{
		#region Constants and Fields

		private static bool isInitialized;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		///     Creates dependency injection kernel for bindings.
		/// </summary>
		/// <returns>IKernel.</returns>
		public static IKernel CreateKernel()
		{
			if (isInitialized)
			{
				isInitialized = true;
				return null;
			}
			var kernel = new StandardKernel();
			RegisterServices(kernel);
			Dependency.Initialize(new NinjectResolver(kernel));
			return kernel;
		}

		#endregion

		#region Methods

		/// <summary>
		///     Registers the services / interface bindings.
		///     Place all DI bindings here:
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			kernel.Bind<ILogger>().To<Log4NetLogger>().InThreadScope();
			kernel.Bind<IViewService>().To<WpfViewService>().InSingletonScope();
			kernel.Bind<ILogListener>().To<LogListener>().InThreadScope();

			
			kernel.Bind<MainViewModel>().ToConstructor(x => new MainViewModel(
				x.Inject<ILogger>(), 
				x.Inject<IViewService>())).InSingletonScope();
			
			
			//kernel.Bind<IUIMarshaller>().To<WpfUIMarshaller>().InSingletonScope();

			kernel.Bind<IUIServiceHost>().To<UIServiceHost>().InSingletonScope();
		}

		#endregion
	}
}