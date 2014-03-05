NUnitBenchmarker
==

A simple to use framework for setting up benchmarks and visualizing results in real time.

Write performance tests as you would normal unit tests with NUnit (with a little twist) and NUnitBenchmarker will take care of the rest.


![NUnitBenchmarker's GUI main screen](docs/img/GUI001.png) 

## Features

Things NUnitBenchmarker can do for you *by writing simple unit test like code snipets*:

- Benchmarks your choosen interface's multiple implementations
- After the test method is written by you NUnutBenchmarker will
    - Look for the given interface's implementations 
        - by configuration
        - by convention (using the current folder's all assemblies)
    - Optionally launches the GUI, where you can load and select more additional implementations
    - Runs the benchmarks you've defined as standard NUnit test methods 
    - Displays result diagrams and data tables in the GUI
    - Creates a PDF report with the diagrams and data tables.
- Can be used by any NUnit runner such as NUnit native runner or JetBrains ReSharper 
- Can run in GUI-less headless mode also
- Fully configurable by standard .NET configuration section

## [Getting Started](docs/GettingStarted.md)

To get an instant picture how can you utilize NUnitBenchmarker please read the [Getting Started](docs/GettingStarted.md) section 

## Roadmap

- Bindings and sample usage for other popular unit test frameworks
- Nuget package
- Visual Studio integrated GUI

## Support

You can ask for support on our mailing list: https://groups.google.com/forum/to_be_created

## Contribute

Everyone is encouraged to contribute, either by:

- Submitting pull requests
- Documentation
- Blogs and tutorials

## License

This project is open source and released under the [MIT license.](LICENSE)

