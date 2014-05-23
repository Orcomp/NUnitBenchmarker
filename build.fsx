#r @"tools\FAKE\tools\FakeLib.dll"

open System
open System.IO
open System.Linq
open System.Text
open System.Text.RegularExpressions
open Fake
open Fake.MSTest

// --------------------------------------------------------------------------------------
// Definitions

let binProjectName = "NUnitBenchmarker.Benchmark"
let netVersions = ["NET40"]

let srcDir  = @".\src\"
let deploymentDir  = @".\deployment\"
// This is the deployed (our) package dir. Do not confuse this with the used packages loacation
// what is called nugetRepositoryDir few lines later
let packagesDir = deploymentDir @@ "packages"

let dllDeploymentDirs = netVersions |> List.map(fun v -> v, packagesDir @@ "work" @@ "lib" @@ v) |> dict
let toolDeploymentDirs = netVersions |> List.map(fun v -> v, packagesDir @@ "work" @@ "tools" @@ v) |> dict
let contentDeploymentDirs = netVersions |> List.map(fun v -> v, packagesDir @@ "work" @@ "content" @@ v) |> dict
let nuspecTemplatesDir = deploymentDir @@ "templates"
let contentTemplatesDir = nuspecTemplatesDir @@ "content"
let toolTemplatesDir = nuspecTemplatesDir @@ "tools"


let nugetExePath = @".\tools\nuget\nuget.exe"
let nugetRepositoryDir = @".\lib"
let nugetAccessKey = if File.Exists(@".\Nuget.key") then File.ReadAllText(@".\Nuget.key") else ""
let version = File.ReadAllText(@".\version.txt")

let solutionAssemblyInfoPath = srcDir @@ "SolutionAssemblyInfo.cs"
let projectsToPackageAssemblyNames = ["NUnitBenchmarker.Benchmark"; "NUnitBenchmarker.Core"; "NUnitBenchmarker.UIClient"; "NUnitBenchmarker.UIService"]
let projectsToToolPackageAssemblyNames = ["NUnitBenchmarker.UI"]
let projectsToPackageDependencies = ["SimpleSpeedTester"; "fasterflect"; "Catel.Core"; "NUnit"; "OxyPlot.Pdf"]

let outputDir = @".\output\"
let outputReleaseDir = outputDir @@ "release" ////@@ netVersion
let outputBinDir = outputReleaseDir ////@@ binProjectName

let getProjectOutputBinDirs netVersion projectName = outputBinDir @@ netVersion @@ projectName
let testResultsDir = srcDir @@ "TestResults"

let ignoreBinFiles = "*.vshost.exe"
let ignoreBinFilesPattern = @"\**\" @@ ignoreBinFiles
let outputBinFiles = !! (outputBinDir @@ @"\**\*.*")
                            -- ignoreBinFilesPattern

let tests = srcDir @@ @"\**\*.Tests.csproj" 
let allProjects = srcDir @@ @"\**\*.csproj" 

let testProjects  = !! tests
let otherProjects = !! allProjects
                        -- tests

// --------------------------------------------------------------------------------------
// Clean build results

Target "CleanPackagesDirectory" (fun _ ->
    CleanDirs [packagesDir; testResultsDir]
)

Target "DeleteOutputFiles" (fun _ ->
    !! (outputDir + @"\**\*.*")
       ++ (testResultsDir + @"\**\*.*")
        -- ignoreBinFilesPattern
    |> DeleteFiles
)

Target "DeleteOutputDirectories" (fun _ ->
    CreateDir outputDir
    DirectoryInfo(outputDir).GetDirectories("*", SearchOption.AllDirectories)
    |> Array.filter (fun d -> not (d.GetFiles(ignoreBinFiles, SearchOption.AllDirectories).Any()))
    |> Array.map (fun d -> d.FullName)
    |> DeleteDirs
)

// --------------------------------------------------------------------------------------
// Build projects

Target "RestorePackagesManually" (fun _ ->
      !! "./**/packages.config"
      |> Seq.iter (RestorePackage (fun p -> 
                  { p with 
                        ToolPath = nugetExePath
                        OutputPath = nugetRepositoryDir
                    }))
)

// TODO: Implement it with 1.0.4-rc like version.txt also.
// Currently we can not use release version numbers because one of our dependencies (Catel) and 
// NuGet build does not allow this package to have a release formatted version attribute.
//Target "UpdateAssemblyVersion" (fun _ ->
//      let pattern = Regex("Assembly(|File)Version(\w*)\(.*\)")
//      let result = "Assembly$1Version$2(\"" + version + "\")"
//      let content = File.ReadAllLines(solutionAssemblyInfoPath, Encoding.Unicode)
//                    |> Array.map(fun line -> pattern.Replace(line, result, 1))
//      File.WriteAllLines(solutionAssemblyInfoPath, content, Encoding.Unicode)
//)
//Target "UpdateAssemblyVersion" DoNothing
Target "UpdateAssemblyVersion" (fun _ ->
      let pattern = Regex("AssemblyInformationalVersion(\w*)\(.*\)")
      let result = "AssemblyInformationalVersion(\"" + version + "\")"
      let content = File.ReadAllLines(solutionAssemblyInfoPath, Encoding.Unicode)
                    |> Array.map(fun line -> pattern.Replace(line, result, 1))
      File.WriteAllLines(solutionAssemblyInfoPath, content, Encoding.Unicode)
)

Target "BuildOtherProjects" (fun _ ->    
    otherProjects
      |> MSBuildRelease "" "Rebuild" 
      |> Log "Build Other Projects"
)

Target "BuildTests" (fun _ ->    
    testProjects
      |> MSBuildRelease "" "Build"
      |> Log "Build Tests"
)

// --------------------------------------------------------------------------------------
// Run tests

Target "RunTests" (fun _ ->
    ActivateFinalTarget "CloseMSTestRunner"
    CleanDir testResultsDir
    CreateDir testResultsDir

    !! (outputDir + @"\**\*.Tests.dll") 
    ++ (srcDir + @"\**\bin\Release\*.Tests.dll") 
      |> MSTest (fun p ->
                  { p with 
                     TimeOut = TimeSpan.FromMinutes 20. 
                     ResultsDir = testResultsDir})
)

FinalTarget "CloseMSTestRunner" (fun _ ->  
    ProcessHelper.killProcess "mstest.exe"
)

// --------------------------------------------------------------------------------------
// Build a NuGet package

Target "NuGet" (fun _ ->
    let nugetAccessPublishKey = getBuildParamOrDefault "nugetkey" nugetAccessKey
    
    let getOutputFile netVersion projectName ext = sprintf @"%s\%s.%s" (getProjectOutputBinDirs netVersion projectName) projectName ext
   

    let getBinProjectFiles netVersion projectName =  [(getOutputFile netVersion projectName "dll")
                                                      (getOutputFile netVersion projectName "exe")
                                                      (getOutputFile netVersion projectName "xml")]

    
    
    let getToolBinProjectFilesExt netVersion projectName ext = 
        (filesInDirMatching ext (directoryInfo (getProjectOutputBinDirs netVersion projectName))) 
            |> Seq.map (fun fi -> fi.FullName) 
            |> List.ofSeq

    let getToolBinProjectFiles netVersion projectName = 
        ["*.dll"; "*.exe"; "*.config"] |> List.collect(fun d -> getToolBinProjectFilesExt netVersion projectName d)

    
    let binProjectFiles netVersion = projectsToPackageAssemblyNames
                                       |> List.collect(fun d -> getBinProjectFiles netVersion d)
                                       |> List.filter(fun d -> File.Exists(d))

    let toolBinProjectFiles netVersion = projectsToToolPackageAssemblyNames
                                       |> List.collect(fun d -> getToolBinProjectFiles netVersion d)
                                       |> List.filter(fun d -> File.Exists(d) )


    let nugetDependencies = projectsToPackageDependencies
                              |> List.map (fun d -> d, GetPackageVersion nugetRepositoryDir d)
    
    let getNuspecFile = sprintf "%s\%s.nuspec" nuspecTemplatesDir binProjectName

    let preparePackage filesToPackage toolFilesToPackage = 
        // Packages:
        CreateDir (packagesDir @@ "work")
        dllDeploymentDirs.Values |> Seq.iter (fun d -> CreateDir d)
        dllDeploymentDirs |> Seq.iter (fun d -> CopyFiles d.Value (filesToPackage d.Key))

        // Tools:
        // Copy UI project output to tools folder
        toolDeploymentDirs.Values |> Seq.iter (fun d -> CreateDir d)
        toolDeploymentDirs |> Seq.iter (fun d -> CopyFiles d.Value (toolFilesToPackage d.Key))
        // Copy template/tools to tools folder (like install.ps1)
        toolDeploymentDirs |> Seq.iter (fun d -> CopyDir d.Value toolTemplatesDir allFiles)
        
        // Content:
        contentDeploymentDirs |> Seq.iter (fun d -> CopyDir d.Value contentTemplatesDir allFiles)
        

    let cleanPackage name = 
        CleanDirs [packagesDir @@ "work"]

    let doPackage dependencies =   
        NuGet (fun p -> 
            {p with
                Project = binProjectName
                Version = version
                ToolPath = nugetExePath
                OutputPath = packagesDir
                WorkingDir = packagesDir @@ "work"
                Dependencies = dependencies
                Publish = not (String.IsNullOrEmpty nugetAccessPublishKey)
                AccessKey = nugetAccessPublishKey })
                getNuspecFile
    
    let doAll files toolFiles dependencies =
        preparePackage files toolFiles
        doPackage dependencies
        cleanPackage ""

    doAll binProjectFiles toolBinProjectFiles nugetDependencies
)

// --------------------------------------------------------------------------------------
// Combined targets

Target "Clean" DoNothing
"CleanPackagesDirectory" ==> "DeleteOutputFiles" ==> "DeleteOutputDirectories" ==> "Clean"

Target "Build" DoNothing
"UpdateAssemblyVersion" ==> "BuildOtherProjects" ==> "Build"

Target "Tests" DoNothing
"BuildTests" ==> "RunTests" ==> "Tests"


Target "All" DoNothing
"Clean" ==> "All"
"Build" ==> "All"
"Tests" ==> "All"

Target "Release" DoNothing
"All" ==> "Release"
"BuildOtherProjects" ==> "Release"
"NuGet" ==> "Release"

//RunTargetOrDefault "Clean" fixed and tested at 23 May
//RunTargetOrDefault "UpdateAssemblyVersion" fixed and tested at 23 May
//RunTargetOrDefault "RestorePackagesManually" fixed and tested at 23 May
//RunTargetOrDefault "BuildOtherProjects" fixed and tested at 23 May
//RunTargetOrDefault "BuildTests"
//RunTargetOrDefault "NuGet"
RunTargetOrDefault "Release"



