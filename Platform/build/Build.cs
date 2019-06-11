using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
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
            void Delete(DirectoryInfo v)
            {
                if (new[] { "node_modules", "packages", "bin", "obj", "generated" }.Contains(v.Name.ToLowerInvariant()))
                {
                    DeleteDirectory(v.FullName);
                    return;
                }

                foreach (var child in v.GetDirectories())
                {
                    Delete(child);
                }
            }

            foreach (var child in new DirectoryInfo(Paths.Platform).GetDirectories().Where(v => !v.Name.Equals("build")))
            {
                Delete(child);
            }

            EnsureCleanDirectory(Paths.Artifacts);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Generate => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.PlatformAdaptersRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.PlatformAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.PlatformAdapters)
                .SetProjectFile(Paths.PlatformAdaptersGenerate));
        });

    Target TestMemory => _ => _
        .DependsOn(Generate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Memory")
                .SetLogger("trx;LogFileName=Memory.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target TestSqlClient => _ => _
        .DependsOn(Generate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Object.SqlClient")
                .SetLogger("trx;LogFileName=SqlClient.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target TestNpgsql => _ => _
        .DependsOn(Generate)
        .Executes(() =>
        {
             DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Adapters.Object.Npgsql")
                .SetLogger("trx;LogFileName=Npgsql.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target Default => _ => _
        .DependsOn(Clean)
        .DependsOn(Generate);
}
