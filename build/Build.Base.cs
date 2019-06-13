using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target BaseGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.BaseRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.BaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseDatabaseGenerate));
        });

    Target BaseDatabaseTestDomain => _ => _
        .DependsOn(BaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.BaseDatabaseDomainTests)
                .SetLogger("trx;LogFileName=BaseDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target BasePublishServer => _ => _
        .DependsOn(BaseGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetWorkingDirectory(Paths.BaseDatabaseServer)
                .SetOutput(Paths.ArtifactsBaseServer);
            DotNetPublish(dotNetPublishSettings);
        });

    Target BaseDatabaseTestServer => _ => _
        .DependsOn(BaseGenerate)
        .DependsOn(BasePublishServer)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Init();
                DotNetTest(s => s
                    .SetProjectFile(Paths.BaseDatabaseServerTests)
                    .SetLogger("trx;LogFileName=BaseDatabaseServer.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    Target BaseWorkspaceNpmInstall => _ => _
        .Executes(() =>
        {
            foreach (var path in Paths.BaseWorkspaceTypescript)
            {
                NpmInstall(s => s
                    .SetWorkingDirectory(path));
            }
        });

    Target BaseWorkspaceSetup => _ => _
        .DependsOn(BaseWorkspaceNpmInstall)
        .DependsOn(BaseGenerate);
    
    Target BaseWorkspaceAutotest => _ => _
        .DependsOn(BaseWorkspaceSetup)
        .Executes(() =>
        {
            foreach (var path in new[] { Paths.BaseWorkspaceTypescriptMaterial, Paths.BaseWorkspaceTypescriptAutotestAngular })
            {
                NpmRun(s => s
                    .SetWorkingDirectory(path)
                    .SetCommand("autotest"));
            }

            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseWorkspaceTypescriptAutotestGenerateGenerate));
        });

    Target BaseWorkspaceTypescriptDomain => _ => _
        .DependsOn(BaseWorkspaceSetup)
        .DependsOn(EnsureDirectories)
        .Executes(() =>
        {
            NpmRun(s => s
                .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptDomain)
                .SetArguments("--reporter-options", $"output={Paths.ArtifactsTestsBaseWorkspaceTypescriptDomain}")
                .SetCommand("az:test"));
        });

    Target BaseWorkspaceTypescriptPromise => _ => _
        .DependsOn(BaseWorkspaceSetup)
        .DependsOn(BasePublishServer)
        .DependsOn(EnsureDirectories)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Init();
                NpmRun(s => s
                    .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptPromise)
                    .SetArguments("--reporter-options", $"output={Paths.ArtifactsTestsBaseWorkspaceTypescriptDomain}")
                    .SetCommand("az:test"));
            }
        });

    Target BaseDatabaseTest => _ => _
        .DependsOn(BaseDatabaseTestDomain)
        .DependsOn(BaseDatabaseTestServer);

    Target BaseWorkspaceTest => _ => _
        .DependsOn(BaseWorkspaceTypescriptDomain)
        .DependsOn(BaseWorkspaceTypescriptPromise);

    Target BaseTest => _ => _
        .DependsOn(BaseDatabaseTest)
        .DependsOn(BaseWorkspaceTest);

    Target Base => _ => _
        .DependsOn(Clean)
        .DependsOn(BaseWorkspaceTypescriptPromise);
}
