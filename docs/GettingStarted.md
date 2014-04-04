Getting Started
==


#### Requirements
* Visual Studio 2012 or 2013 with NuGet package manager.

#### Download NUnitBenchmarker solutions from GitHub
You will need these repositories:
* https://github.com/Orcomp/NUnitBenchmarker

(Later NUnitBenchmarker will be available as Nuget package)

#### Building The Solution

You will need Nuget installed on your computer.

The first time you build the solution, Nuget will fetch the required packages. (If it fails to do this you may have to right click on the solution and enable "Nuget Restore".) and then rebuild the solution again. This time it should work.



#### Architecture

In a typical scenario there will be four participants:

* The implementation under performance test: **The Testee**
* The unit test like method what you are about to write now: **The Test**
* The NUnitBenchmarker helper framework which is a class library assembly: **NUnitBenchmarker**
* The GUI which is a WPF executable: **The GUI**

Your **Test** will reference **NUnitBenchmarker** and will use the static Benchmarker class's helper methods.
That's all. Well... not exactly. You will have to implement a standard NUnit TestCaseSource class which is a factory for drive the test cases.

When your test will run in any runner, it will launch and communicate with **The GUI**. This communication is optional you can run the tests headless and use the output PDFs only



#### Writing a performance test

* Create a unit test project and refer to **NUnitBenchmarker**
* Suppose you have two IList implementations implemented in **The Testee**
* Write a unit test method like this: (Note: we are using only standard NUnit attributes)


```csharp
[Test, TestCaseSource(typeof(ListPerformanceTestFactory<int>), "TestCases")]
public void AddTest(ListPerformanceTestCaseConfiguration<int> conf)
{
	var itemsToAdd = ListPerformanceTestHelper<int>.GenerateItemsToAdd(conf).ToArray();
	var target = ListPerformanceTestHelper<int>.CreateListInstance(conf);
			
	var action = new Action(() =>
	{
		foreach (var item in itemsToAdd)
		{
			target.Add(item);
		}
	});

	action.Benchmark(conf, "Add", conf.ToString());
}
```

* This method above is testing an IList implementation's Add method for different number of items to add.
* The factory class is ListPerformanceTestFactory its factory method is TestCases 
* The Benchmarker static class is called the implemented action like Benchmarker.Bencmark(action, ....) using the extension method syntactic sugar.


* You must implement the ListPerformanceTestFactory class and its TestCase method

```csharp
public class ListPerformanceTestFactory<T> 
{
	public IList<Type> Implementations { get; private set; }

	public ListPerformanceTestFactory()
	{
		Implementations = Benchmarker.GetImplementations(typeof(IList<>), true).ToList();
	}
		 
	public IEnumerable<ListPerformanceTestCaseConfiguration<T>> TestCases()
	{
		// Issue in NUnit: even this method is called _earlier_ than TestFixtureSetup....
		// so we can not call GetImplementations here, because FindImplementatins was not called yet :-(

		var lastImplementation = Implementations.LastOrDefault();

		foreach (var implementation in Implementations)
		{
			var identifier = string.Format("{0}", implementation.GetFriendlyName());

			yield return new ListPerformanceTestCaseConfiguration<T>()
			{
				Identifier = identifier,
				TargetImplementationType = implementation,
				IsLast = false,
				Size = 100,
				DummyForTesting = 0
			};
			// ...more yields with other TestCaseConfiguration data, for exemple initing Size = 1000 etc
		}
	}
}
```

* Notice the GetImplementations helper method in your constructor. All the IList implementations will be there what were configured, or loaded from the current folder, or interactively were selected in the GUI
* Of course you can use creating, filling, and returning an IEnumerable instead of using yield language feature.
* Think about the ListPerformanceTestCaseConfiguration class as a communication (or parameter) class for the Add test.
* You are done. Without configuration all the assemblies will reflected in the current folder, and all IList implementations will be loaded for test. The GUI also will show up.
* For more details see NUnitBenchmarker.Benchmark.Tests project ProofOfConcept.ListPerformanceTest sample

#### Exporting a performance test result to .pdf and .csv

If you want to export the plotted graph to .pdf and the numerical results to .csv you can call the Benchmarker.ExportAllResults() method when all tests finished:

```csharp
[TestFixtureTearDown]
public void TestFixtureTearDown()
{
	Benchmarker.ExportAllResults();
}
```

#### Configuring a performance test

NUnitBenchmarker will look for the given interface's implementations

- by configuration
- by convention (using the current folder's all assemblies)
- optionally launches the GUI, where you can load and select more additional implementations

NUnitBenchmarker has its standard .NET configuration section. To use this place the handler declaration to the test assembly's config file

```xml
<configuration>
  <configSections>
    <section name="NUnitBenchmarkerConfigSection" type="NUnitBenchmarker.Benchmark.Configuration.NUnitBenchmarkerConfigurationSection, NUnitBenchmarker.Benchmark"/>
  </configSections>

  ...

</configuration>
```

Somewhere below place the configuration data:


```xml
<configuration>
  <NUnitBenchmarkerConfigSection displayUI="true">

    <SearchFolders>
      <add Include ="" Exclude="" Folder="." />
    </SearchFolders>

    <ImplementationFilters>
      <add Include ="" Exclude="" />
    </ImplementationFilters> 

    <TestCaseFilters>
      <add Include ="" Exclude="" />
    </TestCaseFilters>
  </NUnitBenchmarkerConfigSection>
</configuration>
```

* Set NUnitBenchmarkerConfigSection element displayUI attribute to false if you would like NUnitBenchmarker to run GUI-less haedless mode. 
You can override this setting by code, see BenchMarker class's GetImplementations(...) method.
* You can add any number of Folder to search within the SearchFolder element. For each Folder you can define regex filter to include or exclude from the particular folder by file name.
* Within all the disovered assemblies you can filter the discowered types with multiple regex include and exclude filters. Pleae note the filters are working in the whole type name includeing the namespaces.
* When the test runtime comes the runner will call your test case factory to create the test cases. Using the TestCaseFilters element you can filter out cases by including or excluding cases by regex name match.

## NUnit issue workarounds

####NUnit issue#1: 
TestFixtureSetup will run **later** than constructor and factory methods of TestCaseSource....so we are late there


We are calling Benchmarker.GetImplementations() to get implementations in the TestCaseSource 
constructor. GetImplementations will message to UI to get the must updated UI selected/deselcted
implementations. However GetImplementations will check if Benchmarker has internally filled discovered
implementations by configuration or by convention. If it is not filled then calls FindImplementations
and fills and caches this implementation list.  GetImplementation() returns always 
A + B where:
A: the lazy discovered (once) (and cached) implementations by configuration or by convention
B: the actual response from UI
Note: Both A and B can be empty.

Please note again: We can not use TestFixtureSetup as "Init" logic because of NUnit runner limitation.


####NUnit issue#2: 

NUnit runner control flow:

* a) TestCaseSource (ListPerformanceTestFactory)instance constructor is called
* b) TestCaseSource (ListPerformanceTestFactory) factory method (TestCases()) is called
* c) ( step a) and b) repeated for all Test methods where [TestCaseSource] attribute was used, **regardless** which Test method was chosen to run. 
* d) Test class static constructor called
* e) TestFixtureSetup called
...
* f) .... and now the Test methods called

Please note c) and the **regardless**. This means a sideeffect for you: 

If you run an other TestFixture's other test which nothing have to do with this TestCaseSource still, the TestCaseSource constructor and factory method will run as a side effect. 

Happy benchmarking :-)










