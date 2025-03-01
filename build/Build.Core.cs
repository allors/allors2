using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target CoreResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Core";
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Drop(database);
                sqlServer.Create(database);
            }
        });

    private Target CoreMerge => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.CoreDatabaseMerge)
                .SetApplicationArguments(Paths.CoreDatabaseResourcesCore, Paths.CoreDatabaseResourcesCustom, Paths.CoreDatabaseResources));
        });

    Target CoreGenerate => _ => _
        .After(Clean)
        .DependsOn(CoreMerge)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments(Paths.CoreRepositoryDomainRepository, Paths.PlatformRepositoryTemplatesMetaCs, Paths.CoreDatabaseMetaGenerated));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.Core)
                .SetProjectFile(Paths.CoreDatabaseGenerate));
        });

    Target CoreDatabaseTestDomain => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.CoreDatabaseDomainTests)
                .AddLoggers("trx;LogFileName=CoreDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target CorePublishCommands => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.CoreDatabaseCommands)
                .SetOutput(Paths.ArtifactsCoreCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    Target CorePublishServer => _ => _
        .DependsOn(CoreGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetProcessWorkingDirectory(Paths.CoreDatabaseServer)
                .SetOutput(Paths.ArtifactsCoreServer);
            DotNetPublish(dotNetPublishSettings);
        });

    Target CoreDatabaseTestServer => _ => _
        .DependsOn(CoreGenerate)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(CoreResetDatabase)
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
                        .AddLoggers("trx;LogFileName=CoreDatabaseServer.trx")
                        .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        });

    Target CoreInstall => _ => _
        .Executes(() =>
        {
            NpmInstall(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.CoreWorkspaceTypescript));
        });

    Target CoreWorkspaceTypescriptDomain => _ => _
        .DependsOn(CoreGenerate)
        .DependsOn(EnsureDirectories)
        .Executes(() =>
        {
            NpmRun(s => s
                .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                .SetProcessWorkingDirectory(Paths.CoreWorkspaceTypescript)
                .SetCommand("domain:test"));
        });

    Target CoreWorkspaceTypescriptPromise => _ => _
        .DependsOn(CoreGenerate)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(EnsureDirectories)
        .DependsOn(CoreResetDatabase)
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
                        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                        .SetProcessWorkingDirectory(Paths.CoreWorkspaceTypescript)
                        .SetCommand("promise:test"));
                }
            }
        });

    Target CoreWorkspaceTypescriptAngular => _ => _
        .DependsOn(CoreGenerate)
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(EnsureDirectories)
        .DependsOn(CoreResetDatabase)
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
                        .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
                        .SetProcessWorkingDirectory(Paths.CoreWorkspaceTypescript)
                        .SetCommand("angular:test"));
                }
            }
        });

    Target CoreWorkspaceCSharpDomainTests => _ => _
        .DependsOn(CorePublishServer)
        .DependsOn(CorePublishCommands)
        .DependsOn(CoreResetDatabase)
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
                        .SetProjectFile(Paths.CoreWorkspaceCSharpDomainTests)
                        .AddLoggers("trx;LogFileName=CoreWorkspaceCSharpDomainTests.trx")
                        .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        });

    Target CoreDatabaseTest => _ => _
        .DependsOn(CoreDatabaseTestDomain)
        .DependsOn(CoreDatabaseTestServer);

    Target CoreWorkspaceTypescriptTest => _ => _
        .DependsOn(CoreWorkspaceTypescriptDomain)
        .DependsOn(CoreWorkspaceTypescriptPromise);

    Target CoreWorkspaceCSharpTest => _ => _
        .DependsOn(CoreWorkspaceCSharpDomainTests);

    Target CoreWorkspaceTest => _ => _
        .DependsOn(CoreWorkspaceCSharpTest)
        .DependsOn(CoreWorkspaceTypescriptTest);

    Target CoreTest => _ => _
        .DependsOn(CoreDatabaseTest)
        .DependsOn(CoreWorkspaceTest);

    Target Core => _ => _
        .DependsOn(Clean)
        .DependsOn(CoreTest);
}
