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
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build 
{
    Target BaseRestore => _ => _
        .Executes(() =>
        {
            foreach (var path in Paths.BaseWorkspaceTypescript)
            {
                NpmInstall(s => s
                    .SetWorkingDirectory(path));
            }
        });

    Target BaseGenerate => _ => _
        .DependsOn(BaseRestore)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.BaseRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.BaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseDatabaseGenerate));

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

    Target BaseDatabaseTestDomain => _ => _
        .DependsOn(BaseGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.BaseDatabaseDomainTests)
                .SetLogger("trx;LogFileName=BaseDatabaseDomain.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target BaseDatabaseTestServer => _ => _
        .DependsOn(BaseGenerate)
        .Executes(async () =>
        {
            var process = await BaseInitServer();

            try
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.BaseDatabaseServerTests)
                    .SetLogger("trx;LogFileName=BaseDatabaseServer.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
            finally
            {
                process.Kill();
            }
        });

    Target BaseWorkspaceTypescriptDomain => _ => _
        .DependsOn(BaseGenerate)
        .Executes(() =>
        {
            NpmRun(s => s
                .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptDomain)
                .SetCommand("az:test"));
        });

    Target BaseWorkspaceTypescriptPromise => _ => _
        .DependsOn(BaseGenerate)
        .Executes(async () =>
        {
            var process = await BaseInitServer();

            try
            {
                NpmRun(s => s
                    .SetWorkingDirectory(Paths.BaseWorkspaceTypescriptPromise)
                    .SetCommand("az:test"));
            }
            finally
            {
                process.Kill();
            }

        });
    Target BaseDatabaseTest => _ => _
        .DependsOn(BaseDatabaseTestDomain)
        .DependsOn(BaseDatabaseTestServer);

    Target BaseWorkspaceTest => _ => _
        .DependsOn(BaseWorkspaceTypescriptDomain)
        .DependsOn(BaseWorkspaceTypescriptPromise);

    Target Base => _ => _
        .DependsOn(Clean)
        .DependsOn(BaseDatabaseTest);
}
