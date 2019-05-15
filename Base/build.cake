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

var repositorySolutionPath = "../Platform/Repository/Repository.sln"; 
var repositoryGenerateProjectPath = "../Platform/Repository/Generate/Generate.csproj";
var databaseSolutionPath = "Database.sln"; 
var databaseGenerateProjectPath = "Database/Generate/Generate.csproj";

var workspaceSolutionPath = "Workspace.sln"; 
var materialTestsSolutionPath = "Material.Tests.sln"; 
var materialTestsProjectPath = "Workspace/Typescript/Material.Tests/Material.Tests.csproj"; 
var autotestSolutionPath = "Workspace/Typescript/Autotest/Autotest.sln";
var autotestGenerateProjectPath = "Workspace/Typescript/Autotest/Generate/Generate.csproj";

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
        MSBuildSettings = new DotNetCoreMSBuildSettings()
            .SetVersion(gitVersion.AssemblySemVer)
            .WithProperty("FileVersion", gitVersion.NuGetVersion)
            .WithProperty("InformationalVersion", gitVersion.NuGetVersion)
            .WithProperty("nowarn", "7035"),
        Verbosity = DotNetCoreVerbosity.Quiet
    };

var dotNetCoreRunSettings = new DotNetCoreRunSettings
    {
        Configuration = configuration,
        Verbosity = DotNetCoreVerbosity.Quiet
    };

var msBuildSettings = new MSBuildSettings{
    Configuration = configuration,
    ToolVersion = MSBuildToolVersion.VS2019,
    Verbosity = Verbosity.Minimal
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

var databaseCleanTask = Task("DatabaseClean")
.Does(() => {
    DotNetCoreClean(repositorySolutionPath, dotNetCoreCleanSettings);
    DotNetCoreClean(databaseSolutionPath, dotNetCoreCleanSettings);

    CleanDirectory("Meta/Generated");
    CleanDirectory("Domain/Generated");
});

var databaseBuildTask = Task("DatabaseBuild")
.Does(() => {
    DotNetCoreRun(repositoryGenerateProjectPath, "Repository/Domain/Repository.csproj ../Platform/Repository/templates/meta.cs.stg Database/Meta/generated", dotNetCoreRunSettings);
    DotNetCoreRun(databaseGenerateProjectPath, string.Empty, dotNetCoreRunSettings);
    DotNetCoreBuild(databaseSolutionPath, dotNetCoreBuildSettings);
});

var databaseTestTask = Task("DatabaseTest")
.Does(() => {
    var dotNetCoreTestSettings = new DotNetCoreTestSettings()
    {
        Configuration = configuration,
    };

    DotNetCoreTest("Database/Domain.Tests/Domain.Tests.csproj", dotNetCoreTestSettings);
});

var workspaceCleanTask = Task("WorkspaceClean")
.Does(() => {
    DotNetCoreClean(workspaceSolutionPath, dotNetCoreCleanSettings);
    DotNetCoreClean(materialTestsSolutionPath, dotNetCoreCleanSettings);

    void NodeClean(string directory){
        DoInDirectory(directory, () => {
            CleanDirectory("node_modules");
            CleanDirectory("dist");
        });
    }

    NodeClean("Workspace/Typescript/Domain");
    NodeClean("Workspace/Typescript/Promise");
    NodeClean("Workspace/Typescript/Angular");
    NodeClean("Workspace/Typescript/Material");
    NodeClean("Workspace/Typescript/Autotest/Angular");

    CleanDirectory("Workspace/Typescript/Material.Tests/generated");
});

var workspaceRestoreTask = Task("WorkspaceRestore")
.Does(() => {
    void NodeRestore(string directory){
        DoInDirectory(directory, () => {
            NpmInstall();
        });
    }

    NodeRestore("Workspace/Typescript/Domain");
    NodeRestore("Workspace/Typescript/Promise");
    NodeRestore("Workspace/Typescript/Angular");
    NodeRestore("Workspace/Typescript/Material");
    NodeRestore("Workspace/Typescript/Autotest/Angular");
});

var workspaceBuildTask = Task("WorkspaceBuild")
.IsDependentOn(workspaceRestoreTask)
.Does(() => {
    void NodeRunAutotest(string directory){
        DoInDirectory(directory, () => {
            NpmRunScript("autotest");
        });
    }

    NodeRunAutotest("Workspace/Typescript/Material");
    NodeRunAutotest("Workspace/Typescript/Autotest/Angular");

    DotNetCoreRun(autotestGenerateProjectPath, string.Empty, dotNetCoreRunSettings);
    NuGetRestore(workspaceSolutionPath);
    MSBuild(workspaceSolutionPath, msBuildSettings);
    DotNetCoreBuild(materialTestsSolutionPath, dotNetCoreBuildSettings);
});

///////////////////////////////////////////////////////////////////////////////
// TASK TARGETS
///////////////////////////////////////////////////////////////////////////////

var cleanTask = Task("Clean")
    .IsDependentOn(databaseCleanTask)
    .IsDependentOn(workspaceCleanTask);

var buildTask = Task("Build")
    .IsDependentOn(databaseBuildTask)
    .IsDependentOn(workspaceBuildTask);

var rebuildTask = Task("Rebuild")
    .IsDependentOn(cleanTask)
    .IsDependentOn(buildTask);

var testTask = Task("Test")
    .IsDependentOn(databaseTestTask);

Task("Default")
    .IsDependentOn(buildTask)
    .Does(()=>{
        Information("NuGetVersion: {0}", gitVersion.NuGetVersion);
        Information("InformationalVersion: {0}", gitVersion.InformationalVersion);
    });

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);