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
            .SetVersion(gitVersion.AssemblySemVer)
            .WithProperty("FileVersion", gitVersion.NuGetVersion)
            .WithProperty("InformationalVersion", gitVersion.NuGetVersion)
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
    DoInDirectory("../Repository", () =>
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
    CleanDirectory("Meta/Generated");
    CleanDirectory("Domain/Generated");

    var repositorySolutionPath = "repository.sln"; 
    DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
    DotNetCoreRestore(repositorySolutionPath, dotNetCoreRestoreSettings);
    DotNetCoreExecute($"../Repository/Generate/bin/{configuration}/{framework}/Generate.dll", "Repository/Domain/Repository.csproj ../repository/templates/meta.cs.stg ./meta/generated");

    var solutionPath = "Adapters.sln"; 
    DotNetCoreClean(solutionPath, dotNetCoreCleanSettings);
    DotNetCoreRestore(solutionPath, dotNetCoreRestoreSettings);

    var generateProjectPath = "Generate/Generate.csproj";
    DotNetCoreBuild(generateProjectPath, dotNetCoreBuildSettings);
    DotNetCoreExecute($"Generate/bin/{configuration}/{framework}/Generate.dll");

    DotNetCoreBuild(solutionPath, dotNetCoreBuildSettings);
});

var testMemoryTask = Task("TestMemory")
.IsDependentOn(adaptersTask)
.Does(() => {
    var dotNetCoreTestSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
        Filter = "FullyQualifiedName~Memory"
    };

    DotNetCoreTest("Tests.Static/Tests.Static.csproj", dotNetCoreTestSettings);
});

var testSqlClientTask = Task("TestSqlClient")
.IsDependentOn(adaptersTask)
.Does(() => {
    var dotNetCoreTestSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
        Filter = "FullyQualifiedName~SqlClient"
    };

    DotNetCoreTest("Tests.Static/Tests.Static.csproj", dotNetCoreTestSettings);
});

var testNpgsqlTask = Task("TestNpgsql")
.IsDependentOn(adaptersTask)
.Does(() => {
    var dotNetCoreTestSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
        NoBuild = true,
        NoRestore = true,
        Filter = "FullyQualifiedName~Npgsql"
    };

    DotNetCoreTest("Tests.Static/Tests.Static.csproj", dotNetCoreTestSettings);
});

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

var testTask = Task("Test")
    .IsDependentOn(testMemoryTask)
    .IsDependentOn(testSqlClientTask)
    .IsDependentOn(testNpgsqlTask);

Task("Default")
    .IsDependentOn(adaptersTask)
    .Does(()=>{
        Information("NuGetVersion: {0}", gitVersion.NuGetVersion);
        Information("InformationalVersion: {0}", gitVersion.InformationalVersion);
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);