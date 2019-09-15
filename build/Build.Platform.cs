using System.IO;
using System.Linq;
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

partial class Build
{
    Target AdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.PlatformAdaptersRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.PlatformAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetWorkingDirectory(Paths.PlatformAdapters)
                .SetProjectFile(Paths.PlatformAdaptersGenerate));
        });

    Target AdaptersTestMemory => _ => _
        .DependsOn(AdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
                .SetLogger("trx;LogFileName=AdaptersMemory.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target AdaptersTestSqlClient => _ => _
        .DependsOn(AdaptersGenerate)
        .Executes(() =>
        {
            using (var database = new SqlServer())
            {
                database.Restart();
                DotNetTest(s => s
                    .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.SqlClient")
                    .SetLogger("trx;LogFileName=AdaptersSqlClient.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    Target AdaptersTestNpgsql => _ => _
        .DependsOn(AdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
               .SetProjectFile(Paths.PlatformAdaptersStaticTests)
               .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Npgsql")
               .SetLogger("trx;LogFileName=AdaptersNpgsql.trx")
               .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target Adapters => _ => _
        .DependsOn(Clean)
        .DependsOn(AdaptersTestMemory)
        .DependsOn(AdaptersTestSqlClient)
        .DependsOn(AdaptersTestNpgsql);
}
