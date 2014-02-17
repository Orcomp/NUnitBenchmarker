using NUnitBenchmarker.Core.Infrastructure.DependencyInjection;

namespace NUnitBenchmarker.UI.ViewModels
{
	/// <summary>
	///     This class contains static references to all the view models in the
	///     application and provides an entry point for the bindings.
	/// </summary>
	public class ViewModelLocator
	{

		public MainViewModel Main
		{
			get
			{
				return Dependency.Resolve<MainViewModel>();
			}
		}

		public DataTabViewModel DataTab
		{
			get
			{
				return Dependency.Resolve<DataTabViewModel>();
			}
		}

		public static void Cleanup()
		{
			// TODO Clear the ViewModels
		}
	}
}