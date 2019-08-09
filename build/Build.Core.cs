using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target CoreGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.CoreRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.CoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Core)
                .SetProjectFile(Paths.CoreDatabaseGenerate));
        });

    Target CoreDatabaseTestDomain => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.CoreDatabaseDomainTests)
                .SetLogger("trx;LogFileName=CoreDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target CorePublishCommands => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetWorkingDirectory(Paths.CoreDatabaseCommands)
                .SetOutput(Paths.ArtifactsCoreCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    Target CorePublishServer => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetWorkingDirectory(Paths.CoreDatabaseServer)
                .SetOutput(Paths.ArtifactsCoreServer);
            DotNetPublish(dotNetPublishSettings);
        });

    Target CoreDatabaseTestServer => _ => _
        .DependsOn(CoreGenerate)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .Executes(async () =>
        {
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Populate(Paths.ArtifactsCoreCommands);
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    DotNetTest(s => s
                        .SetProjectFile(Paths.CoreDatabaseServerTests)
                        .SetLogger("trx;LogFileName=CoreDatabaseServer.trx")
                        .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        });

    Target CoreWorkspaceNpmInstall => _ => _
        .Executes(() =>
        {
            foreach (var path in Paths.CoreWorkspaceTypescript)
            {
                NpmInstall(s => s
                    .SetEnvironmentVariable("npm_config_loglevel", "error")
                    .SetWorkingDirectory(path));
            }
        });

    Target CoreWorkspaceSetup => _ => _
        .DependsOn(CoreWorkspaceNpmInstall)
        .DependsOn(CoreGenerate);

    Target CoreWorkspaceAutotest => _ => _
        .DependsOn(CoreWorkspaceSetup)
        .Executes(() =>
        {
            foreach (var path in new[] { Paths.CoreWorkspaceTypescriptMaterial, Paths.CoreWorkspaceTypescriptAutotestAngular })
            {
                NpmRun(s => s
                    .SetEnvironmentVariable("npm_config_loglevel", "error")
                    .SetWorkingDirectory(path)
                    .SetCommand("autotest"));
            }

            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Core)
                .SetProjectFile(Paths.CoreWorkspaceTypescriptAutotestGenerateGenerate));
        });

    Target CoreWorkspaceTypescriptDomain => _ => _
        .DependsOn(CoreWorkspaceSetup)
        .DependsOn(EnsureDirectories)
        .Executes(() =>
        {
            NpmRun(s => s
                .SetEnvironmentVariable("npm_config_loglevel", "error")
                .SetWorkingDirectory(Paths.CoreWorkspaceTypescriptDomain)
                .SetArguments("--reporter-options", $"output={Paths.ArtifactsTestsCoreWorkspaceTypescriptDomain}")
                .SetCommand("az:test"));
        });

    Target CoreWorkspaceTypescriptPromise => _ => _
        .DependsOn(CoreWorkspaceSetup)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(EnsureDirectories)
        .Executes(async () =>
        {
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Populate(Paths.ArtifactsCoreCommands);
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    NpmRun(s => s
                        .SetEnvironmentVariable("npm_config_loglevel", "error")
                        .SetWorkingDirectory(Paths.CoreWorkspaceTypescriptPromise)
                        .SetArguments("--reporter-options", $"output={Paths.ArtifactsTestsCoreWorkspaceTypescriptPromise}")
                        .SetCommand("az:test"));
                }
            }
        });

    Target CoreWorkspaceTypescriptAngular => _ => _
        .DependsOn(CoreWorkspaceSetup)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(EnsureDirectories)
        .Executes(async () =>
        {
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Populate(Paths.ArtifactsCoreCommands);
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    NpmRun(s => s
                        .SetEnvironmentVariable("npm_config_loglevel", "error")
                        .SetWorkingDirectory(Paths.CoreWorkspaceTypescriptAngular)
                        .SetArguments("--watch=false", "--reporters", "trx")
                        .SetCommand("test"));
                    CopyFileToDirectory(Paths.CoreWorkspaceTypescriptAngularTrx, Paths.ArtifactsTests,
                        FileExistsPolicy.Overwrite);
                }
            }
        });

    Target CoreWorkspaceTypescriptMaterial => _ => _
        .DependsOn(CoreWorkspaceSetup)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .Executes(async () =>
        {
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Populate(Paths.ArtifactsCoreCommands);
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    await server.Ready();
                    NpmRun(s => s
                        .SetEnvironmentVariable("npm_config_loglevel", "error")
                        .SetWorkingDirectory(Paths.CoreWorkspaceTypescriptMaterial)
                        .SetArguments("--watch=false", "--reporters", "trx")
                        .SetCommand("test"));
                    CopyFileToDirectory(Paths.CoreWorkspaceTypescriptMaterialTrx, Paths.ArtifactsTests,
                        FileExistsPolicy.Overwrite);
                }
            }
        });

    Target CoreWorkspaceTypescriptMaterialTests => _ => _
        .DependsOn(CoreWorkspaceAutotest)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .Executes(async () =>
        {
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Populate(Paths.ArtifactsCoreCommands);
                using (var server = new Server(Paths.ArtifactsCoreServer))
                {
                    using (var angular = new Angular(Paths.CoreWorkspaceTypescriptMaterial))
                    {
                        await server.Ready();
                        await angular.Init();
                        DotNetTest(s => s
                            .SetProjectFile(Paths.CoreWorkspaceTypescriptMaterialTests)
                            .SetLogger("trx;LogFileName=CoreWorkspaceTypescriptMaterialTests.trx")
                            .SetResultsDirectory(Paths.ArtifactsTests));
                    }
                }
            }
        });

    Target CoreDatabaseTest => _ => _
        .DependsOn(CoreDatabaseTestDomain)
        .DependsOn(CoreDatabaseTestServer);

    Target CoreWorkspaceTest => _ => _
        .DependsOn(CoreWorkspaceTypescriptDomain)
        .DependsOn(CoreWorkspaceTypescriptPromise)
        .DependsOn(CoreWorkspaceTypescriptAngular)
        .DependsOn(CoreWorkspaceTypescriptMaterial)
        .DependsOn(CoreWorkspaceTypescriptMaterialTests);

    Target CoreTest => _ => _
        .DependsOn(CoreDatabaseTest)
        .DependsOn(CoreWorkspaceTest);

    Target Core => _ => _
        .DependsOn(Clean)
        .DependsOn(CoreTest);
}
