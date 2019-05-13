///////////////////////////////////////////////////////////////////////////////
// This is the main build file.
//
// The following targets are the prefered entrypoints:
// * Build
//
// You can call these targets by using the bootstrapper powershell script
// next to this file: ./build -target <target>
///////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////
// MODULES, TOOLS AND ADDINS
///////////////////////////////////////////////////////////////////////////////

#tool nuget:?package=GitVersion.CommandLine&version=4.0.0
#addin nuget:?package=Cake.Npm&version=0.17.0
#addin nuget:?package=Cake.DoInDirectory&version=3.3.0

///////////////////////////////////////////////////////////////////////////////
// COMMAND LINE ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var framework = Argument( "framework", "netcoreapp2.2" );

///////////////////////////////////////////////////////////////////////////////
// PREPARATION
///////////////////////////////////////////////////////////////////////////////

GitVersion gitVersion = GitVersion(new GitVersionSettings{ NoFetch = true, OutputType = GitVersionOutput.Json });
GitVersion(new GitVersionSettings{OutPutType = GitVersionOutput.BuildServer});
var assemblyVersion = gitVersion.AssemblySemVer;
var packageVersion = gitVersion.NuGetVersion;

var solutions = GetFiles("./**/*.sln");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());

///////////////////////////////////////////////////////////////////////////////
// SETTINGS
///////////////////////////////////////////////////////////////////////////////

var dotNetCoreCleanSettings = new DotNetCoreCleanSettings
    {
        Configuration = configuration
    };

var dotNetCoreRestoreSettings  = new DotNetCoreRestoreSettings 
    {
    };

var dotNetCoreBuildSettings = new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        NoIncremental = true,
        NoRestore = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .SetVersion(assemblyVersion)
            .WithProperty("FileVersion", packageVersion)
            .WithProperty("InformationalVersion", packageVersion)
            .WithProperty("nowarn", "7035")
    };

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

var repositoryTask = Task("Repository")
.Does(() => {
    DoInDirectory("./Core/Repository", () =>
    {
        var solutionPath = "Repository.sln"; 
        DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
        DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);
        DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
    });
});

var adaptersTask = Task("Adapters")
.IsDependentOn(repositoryTask)
.Does(() => {
    DoInDirectory("./Core/Database/Adapters", () =>
    {
        CleanDirectory("Meta/Generated");
        CleanDirectory("Domain/Generated");

        DoInDirectory("./Repository", () =>
        {
            var repositorySolutionPath = "../repository.sln"; 
            DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
            DotNetCoreRestore(repositorySolutionPath, dotNetCoreRestoreSettings);
            DotNetCoreExecute($"../../../Repository/Generate/bin/{configuration}/{framework}/Generate.dll", "repository.csproj ../../../repository/templates/meta.cs.stg ../meta/generated");
        });
  
        var solutionPath = "Adapters.sln"; 
        DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
        DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);

        var generateProjectPath = "Generate/Generate.csproj";
        DotNetCoreBuild(generateProjectPath, dotNetCoreBuildSettings);
        DotNetCoreExecute($"Generate/bin/{configuration}/{framework}/Generate.dll");

        DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
    });
});

var baseTask = Task("Base")
.IsDependentOn(repositoryTask)
.Does(() => {
    DoInDirectory("./Base", () =>
    {
        CleanDirectory("Meta/Generated");
        CleanDirectory("Domain/Generated");

        DoInDirectory("./Repository/Domain", () =>
        {
            var repositorySolutionPath = "../../repository.sln"; 
            DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
            DotNetCoreRestore(repositorySolutionPath, dotNetCoreRestoreSettings);
            DotNetCoreExecute($"../../../Core/Repository/Generate/bin/{configuration}/{framework}/Generate.dll", "repository.csproj ../../../Core/repository/templates/meta.cs.stg ../meta/generated");
        });
  
        var solutionPath = "Database.sln"; 
        DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
        DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);

        var generateProjectPath = "Database/Generate/Generate.csproj";
        DotNetCoreBuild(generateProjectPath, dotNetCoreBuildSettings);
        DotNetCoreExecute($"Database/Generate/bin/{configuration}/{framework}/Generate.dll");

        DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
    });
});

var appsTask = Task("Apps")
.IsDependentOn(repositoryTask)
.Does(() => {
    DoInDirectory("./Apps", () =>
    {
        CleanDirectory("Meta/Generated");
        CleanDirectory("Domain/Generated");

        DoInDirectory("./Repository/Domain", () =>
        {
            var repositorySolutionPath = "../../repository.sln"; 
            DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
            DotNetCoreRestore(repositorySolutionPath, dotNetCoreRestoreSettings);
            DotNetCoreExecute($"../../../Core/Repository/Generate/bin/{configuration}/{framework}/Generate.dll", "repository.csproj ../../../Core/repository/templates/meta.cs.stg ../meta/generated");
        });
  
        var solutionPath = "Database.sln"; 
        DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
        DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);

        var generateProjectPath = "Database/Generate/Generate.csproj";
        DotNetCoreBuild(generateProjectPath, dotNetCoreBuildSettings);
        DotNetCoreExecute($"Database/Generate/bin/{configuration}/{framework}/Generate.dll");

        DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
    });
});

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

var buildTask = Task("Build")
    .IsDependentOn(adaptersTask)
    .IsDependentOn(baseTask)
    .IsDependentOn(appsTask);

Task("Default")
    .IsDependentOn(appsTask)
    .Does(()=>{
        Information("NuGetVersion: {0}", gitVersion.NuGetVersion);
        Information("InformationalVersion: {0}", gitVersion.InformationalVersion);
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);