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

    Target BasePublishCommands => _ => _
        .DependsOn(BaseGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetWorkingDirectory(Paths.BaseDatabaseCommands)
                .SetOutput(Paths.ArtifactsBaseCommands);
            DotNetPublish(dotNetPublishSettings);
        });

    Target BaseCommandsPopulate => _ => _
        .DependsOn(BasePublishCommands)
        .Executes(() =>
        {
            using (var database = new SqlServer())
            {
                database.Restart();
                DotNet("Commands.dll Populate", Paths.ArtifactsBaseCommands);
            }
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
        .DependsOn(BaseCommandsPopulate)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Ready();
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
        .DependsOn(BaseCommandsPopulate)
        .DependsOn(EnsureDirectories)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Ready();
                NpmRun(s => s
                    .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptPromise)
                    .SetArguments("--reporter-options",
                        $"output={Paths.ArtifactsTestsBaseWorkspaceTypescriptPromise}")
                    .SetCommand("az:test"));
            }
        });

    Target BaseWorkspaceTypescriptAngular => _ => _
        .DependsOn(BaseWorkspaceSetup)
        .DependsOn(BasePublishServer)
        .DependsOn(BaseCommandsPopulate)
        .DependsOn(EnsureDirectories)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Ready();
                NpmRun(s => s
                    .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptAngular)
                    .SetArguments("--watch=false", "--reporters", "trx")
                    .SetCommand("test"));
                CopyFileToDirectory(Paths.BaseWorkspaceTypescriptAngularTrx, Paths.ArtifactsTests,
                    FileExistsPolicy.Overwrite);
            }
        });

    Target BaseWorkspaceTypescriptMaterial => _ => _
        .DependsOn(BaseWorkspaceSetup)
        .DependsOn(BasePublishServer)
        .DependsOn(BaseCommandsPopulate)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                await server.Ready();
                NpmRun(s => s
                    .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptMaterial)
                    .SetArguments("--watch=false", "--reporters", "trx")
                    .SetCommand("test"));
                CopyFileToDirectory(Paths.BaseWorkspaceTypescriptMaterialTrx, Paths.ArtifactsTests,
                    FileExistsPolicy.Overwrite);
            }
        });

    Target BaseWorkspaceTypescriptMaterialTests => _ => _
        .DependsOn(BaseWorkspaceAutotest)
        .DependsOn(BasePublishServer)
        .DependsOn(BaseCommandsPopulate)
        .Executes(async () =>
        {
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                using (var angular = new Angular(Paths.BaseWorkspaceTypescriptMaterial))
                {
                    await server.Ready();
                    await angular.Init();
                    DotNetTest(s => s
                        .SetProjectFile(Paths.BaseWorkspaceTypescriptMaterialTests)
                        .SetLogger("trx;LogFileName=BaseWorkspaceTypescriptMaterialTests.trx")
                        .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        });

    Target BaseDatabaseTest => _ => _
        .DependsOn(BaseDatabaseTestDomain)
        .DependsOn(BaseDatabaseTestServer);

    Target BaseWorkspaceTest => _ => _
        .DependsOn(BaseWorkspaceTypescriptDomain)
        .DependsOn(BaseWorkspaceTypescriptPromise)
        .DependsOn(BaseWorkspaceTypescriptAngular)
        .DependsOn(BaseWorkspaceTypescriptMaterial)
        .DependsOn(BaseWorkspaceTypescriptMaterialTests);

    Target BaseTest => _ => _
        .DependsOn(BaseDatabaseTest)
        .DependsOn(BaseWorkspaceTest);

    Target Base => _ => _
        .DependsOn(Clean)
        .DependsOn(BaseTest);
}
