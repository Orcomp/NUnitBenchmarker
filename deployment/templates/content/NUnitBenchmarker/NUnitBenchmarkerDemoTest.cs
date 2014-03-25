using NUnit.Framework;
using NUnitBenchmarker.Benchmark;
using NUnitBenchmarker.UIClient;

// NUnitBenchmarker.log4net.config was added to your project root, and will be copied to the output folder
// during build. You can also add the appropriate log4net config to your app.config 
// and delete NUnitBenchmarker.log4net.config if you prefer.
// 
// You can move this line to your any .cs source, preferably to AssemblyInfo.cs.
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "NUnitBenchmarker.log4net.config", Watch = true)]

namespace NUnitBenchmarker
{
	[TestFixture]
	public class NUnitBenchmarkerDemoTest
	{
		[Test]
		public void TestPing()
		{
			// To use the UI (from any runner, or any host process) simply do the following:
			// 1) Reference the NUnitBenchmarker.UIClient assembly (already done by the installer)
			// 2) Use the static UI class in it:

			Benchmarker.Init();

			var response = UI.Ping("Hello from the runner.");

			// DisplayUI can be configured in app.config. 
			// After the package installation it is set to true in your app.config
			// For further app.config setting see your app.config's NUnitBenchmarkerConfigSection element
			if (UI.DisplayUI)
			{
				Assert.AreEqual("Welcome to the machine: Hello from the runner.", response);
			}
			else
			{
				Assert.AreEqual("Welcome to the loopback: Hello from the runner.", response);
			}
		}
	}
}
