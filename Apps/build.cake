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
GitVersion(new GitVersionSettings{OutputType = GitVersionOutput.BuildServer});

///////////////////////////////////////////////////////////////////////////////
// SETTINGS
///////////////////////////////////////////////////////////////////////////////

var dotNetCoreCleanSettings = new DotNetCoreCleanSettings
    {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Quiet
    };

var dotNetCoreRestoreSettings  = new DotNetCoreRestoreSettings 
    {
        Verbosity = DotNetCoreVerbosity.Quiet
    };

var dotNetCoreBuildSettings = new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        NoIncremental = true,
        NoRestore = true,
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .SetVersion(gitVersion.AssemblySemVer)
            .WithProperty("FileVersion", gitVersion.NuGetVersion)
            .WithProperty("InformationalVersion", gitVersion.NuGetVersion)
            .WithProperty("nowarn", "7035"),
        Verbosity = DotNetCoreVerbosity.Quiet
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
    DoInDirectory("../Platform/Repository", () =>
    {
        var solutionPath = "Repository.sln"; 
        DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
        DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);
        DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
    });
});

var databaseTask = Task("Database")
.IsDependentOn(repositoryTask)
.Does(() => {
    CleanDirectory("Meta/Generated");
    CleanDirectory("Domain/Generated");

    var repositorySolutionPath = "repository.sln"; 
    DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
    DotNetCoreRestore(repositorySolutionPath, dotNetCoreRestoreSettings);
    DotNetCoreExecute($"../Platform/Repository/Generate/bin/{configuration}/{framework}/Generate.dll", "Repository/Domain/Repository.csproj ../Platform/repository/templates/meta.cs.stg Database/meta/generated");

    var solutionPath = "Database.sln"; 
    DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
    DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);

    var generateProjectPath = "Database/Generate/Generate.csproj";
    DotNetCoreBuild(generateProjectPath, dotNetCoreBuildSettings);
    DotNetCoreExecute($"Database/Generate/bin/{configuration}/{framework}/Generate.dll");

    DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
});

var databaseTestTask = Task("DatabaseTest")
.IsDependentOn(databaseTask)
.Does(() => {
    var dotNetCoreTestSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
    };

    DotNetCoreTest("Database/Domain.Tests/Domain.Tests.csproj", dotNetCoreTestSettings);
});

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn(databaseTask)
    .Does(()=>{
        Information("NuGetVersion: {0}", gitVersion.NuGetVersion);
        Information("InformationalVersion: {0}", gitVersion.InformationalVersion);
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);