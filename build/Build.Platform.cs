using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target AdaptersResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Adapters";
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Drop(database);
                sqlServer.Create(database);
            }
        });

    Target AdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.PlatformAdaptersRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.PlatformAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.PlatformAdapters)
                .SetProjectFile(Paths.PlatformAdaptersGenerate));
        });

    Target AdaptersTestMemory => _ => _
        .DependsOn(AdaptersGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
                .AddLoggers("trx;LogFileName=AdaptersMemory.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target AdaptersTestSqlClient => _ => _
        .DependsOn(AdaptersGenerate)
        .DependsOn(AdaptersResetDatabase)
        .Executes(() =>
        {
            using (var database = new SqlServer())
            {
                database.Restart();
                DotNetTest(s => s
                    .SetProjectFile(Paths.PlatformAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.SqlClient")
                    .AddLoggers("trx;LogFileName=AdaptersSqlClient.trx")
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
               .AddLoggers("trx;LogFileName=AdaptersNpgsql.trx")
               .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target Adapters => _ => _
        .DependsOn(Clean)
        .DependsOn(AdaptersTestMemory)
        .DependsOn(AdaptersTestSqlClient)
        .DependsOn(AdaptersTestNpgsql);
}
