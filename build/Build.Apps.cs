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
    Target AppsGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.AppsRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.AppsDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Apps)
                .SetProjectFile(Paths.AppsDatabaseGenerate));
        });

    Target AppsDatabaseTestDomain => _ => _
        .DependsOn(AppsGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.AppsDatabaseDomainTests)
                .SetLogger("trx;LogFileName=AppsDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target AppsPublishServer => _ => _
        .DependsOn(AppsGenerate)
        .Executes(() =>
        {
            var dotNetPublishSettings = new DotNetPublishSettings()
                .SetWorkingDirectory(Paths.AppsDatabaseServer)
                .SetOutput(Paths.ArtifactsAppsServer);
            DotNetPublish(dotNetPublishSettings);
        });

    Target AppsWorkspaceNpmInstall => _ => _
        .Executes(() =>
        {
            foreach (var path in Paths.AppsWorkspaceTypescript)
            {
                NpmInstall(s => s
                    .SetWorkingDirectory(path));
            }
        });

    Target AppsWorkspaceSetup => _ => _
        .DependsOn(AppsWorkspaceNpmInstall)
        .DependsOn(AppsGenerate);

    Target AppsWorkspaceAutotest => _ => _
        .DependsOn(AppsWorkspaceSetup)
        .Executes(() =>
        {
            foreach (var path in new[] { Paths.AppsWorkspaceTypescriptIntranet, Paths.AppsWorkspaceTypescriptAutotestAngular })
            {
                NpmRun(s => s
                    .SetWorkingDirectory(path)
                    .SetCommand("autotest"));
            }

            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Apps)
                .SetProjectFile(Paths.AppsWorkspaceTypescriptAutotestGenerateGenerate));
        });

    Target AppsWorkspaceTypescriptDomain => _ => _
        .DependsOn(AppsWorkspaceSetup)
        .DependsOn(EnsureDirectories)
        .Executes(() =>
        {
            NpmRun(s => s
                .SetWorkingDirectory(Paths.AppsWorkspaceTypescriptDomain)
                .SetArguments("--reporter-options", $"output={Paths.ArtifactsTestsAppsWorkspaceTypescriptDomain}")
                .SetCommand("az:test"));
        });

    Target AppsWorkspaceTypescriptIntranet => _ => _
        //.DependsOn(AppsWorkspaceSetup)
        //.DependsOn(AppsPublishServer)
        .Executes(async () =>
        {
            using (var database = new SqlServer())
            {
                database.Restart();
                using (var server = new Server(Paths.ArtifactsAppsServer))
                {
                    await server.Init();
                    NpmRun(s => s
                        .SetWorkingDirectory(Paths.AppsWorkspaceTypescriptIntranet)
                        .SetArguments("--watch=false", "--reporters", "trx")
                        .SetCommand("test"));
                    CopyFileToDirectory(Paths.AppsWorkspaceTypescriptIntranetTrx, Paths.ArtifactsTests,
                        FileExistsPolicy.Overwrite);
                }
            }
        });

    Target AppsWorkspaceTypescriptIntranetTests => _ => _
        .DependsOn(AppsWorkspaceAutotest)
        .DependsOn(AppsPublishServer)
        .Executes(async () =>
        {
            using (var database = new SqlServer())
            {
                database.Restart();
                using (var server = new Server(Paths.ArtifactsAppsServer))
                {
                    using (var angular = new Angular(Paths.AppsWorkspaceTypescriptIntranet))
                    {
                        await server.Init();
                        await angular.Init();
                        DotNetTest(s => s
                            .SetProjectFile(Paths.AppsWorkspaceTypescriptIntranetTests)
                            .SetLogger("trx;LogFileName=AppsWorkspaceTypescriptMaterialTests.trx")
                            .SetResultsDirectory(Paths.ArtifactsTests));
                    }
                }
            }
        });

    Target AppsDatabaseTest => _ => _
        .DependsOn(AppsDatabaseTestDomain);

    Target AppsWorkspaceTest => _ => _
        .DependsOn(AppsWorkspaceTypescriptDomain)
        .DependsOn(AppsWorkspaceTypescriptIntranet)
        .DependsOn(AppsWorkspaceTypescriptIntranetTests);

    Target AppsTest => _ => _
        .DependsOn(AppsDatabaseTest)
        .DependsOn(AppsWorkspaceTest);

    Target Apps => _ => _
        .DependsOn(Clean)
        .DependsOn(AppsTest);
}
