using System.IO;
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

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    public readonly Paths Paths = new Paths(RootDirectory);

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            foreach (var root in new[] { Paths.Platform, Paths.Core, Paths.Base, Paths.Apps })
            {
                foreach (var pattern in new[] { "node_modules", "packages", "bin", "obj", "generated" })
                {
                    foreach (var directory in Directory.EnumerateDirectories(root, pattern, SearchOption.AllDirectories))
                    {
                        DeleteDirectory(directory);
                    }
                }
            }

            EnsureCleanDirectory(Paths.Artifacts);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));

            foreach (var path in Paths.BaseWorkspaceTypescript)
            {
                NpmTasks.NpmInstall(s => s
                    .SetWorkingDirectory(path));
            }
        });

    Target Generate => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            // Platform 
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.PlatformAdaptersRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.PlatformAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.PlatformAdapters)
                .SetProjectFile(Paths.PlatformAdaptersGenerate));

            // Core 
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.CoreRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.CoreDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Core)
                .SetProjectFile(Paths.CoreDatabaseGenerate));

            // Base 
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.BaseRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.BaseDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseDatabaseGenerate));

            foreach (var path in new[] { Paths.BaseWorkspaceTypescriptMaterial, Paths.BaseWorkspaceTypescriptAutotestAngular })
            {
                NpmTasks.NpmRun(s => s
                    .SetWorkingDirectory(path)
                    .SetCommand("autotest"));
            }

            DotNetRun(s => s
                .SetWorkingDirectory(Paths.Base)
                .SetProjectFile(Paths.BaseWorkspaceTypescriptAutotestGenerateGenerate));
        });
    
    Target TestAdapters => _ => _
        .DependsOn(Generate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Memory")
                .SetLogger("trx;LogFileName=AdaptersMemory.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));

            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Object.SqlClient")
                .SetLogger("trx;LogFileName=AdaptersSqlClient.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));

            /*
             DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Object.Npgsql")
                .SetLogger("trx;LogFileName=AdaptersNpgsql.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
            */
        });

    Target Adapters => _ => _
        .DependsOn(Clean)
        .DependsOn(TestAdapters);

    Target Default => _ => _
        .DependsOn(Generate);
}
