param($installPath, $toolsPath, $package, $project)
$path = [System.IO.Path]

# set 'Copy To Output Directory' to 'Copy if newer'
$copyToOutput = $file.Properties.Item("CopyToOutputDirectory")
$copyToOutput.Value = 2

$testfile = $path::Combine($path::GetDirectoryName($project.FileName), "NUnitBenchmarker\NUnitBenchmarkerDemoTest.cs")
$dte.ItemOperations.OpenFile($testfile)
