using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    private Target DotnetSystemAdaptersGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.DotnetSystemRepositoryGenerate)
                .SetApplicationArguments(
                    $"{Paths.DotnetSystemAdaptersRepositoryDomainRepository} {Paths.DotnetSystemRepositoryTemplatesMetaCs} {Paths.DotnetSystemAdaptersMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.DotnetSystemAdapters)
                .SetProjectFile(Paths.DotnetSystemAdaptersGenerate));
        });

    private Target DotnetSystemAdaptersTestMemory => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() => DotNetTest(s => s
            .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
            .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Memory")
            .AddLoggers("trx;LogFileName=AdaptersMemory.trx")
            .SetResultsDirectory(Paths.ArtifactsTests)));

    private Target DotnetSystemAdaptersTestSqlClient => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new SqlServer())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.SqlClient")
                    .AddLoggers("trx;LogFileName=AdaptersSqlClient.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemAdaptersTestNpgsql => _ => _
        .DependsOn(DotnetSystemAdaptersGenerate)
        .Executes(() =>
        {
            using (new Postgres())
            {
                DotNetTest(s => s
                    .SetProjectFile(Paths.DotnetSystemAdaptersStaticTests)
                    .SetFilter("FullyQualifiedName~Allors.Database.Adapters.Sql.Npgsql")
                    .AddLoggers("trx;LogFileName=AdaptersNpgsql.trx")
                    .SetResultsDirectory(Paths.ArtifactsTests));
            }
        });

    private Target DotnetSystemInstall => _ => _
        .Executes(() => NpmInstall(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.DotnetSystemWorkspaceTypescript)));

    private Target DotnetSystemWorkspaceTypescript => _ => _
        .After(DotnetSystemInstall)
        .DependsOn(EnsureDirectories)
        .Executes(() => NpmRun(s => s
            .AddProcessEnvironmentVariable("npm_config_loglevel", "error")
            .SetProcessWorkingDirectory(Paths.DotnetSystemWorkspaceTypescript)
            .SetCommand("test:all")));
    
    private Target DotnetSystemWorkspaceTest => _ => _
        .DependsOn(DotnetSystemWorkspaceTypescript);

    private Target DotnetSystemAdapters => _ => _
        .DependsOn(Clean)
        .DependsOn(DotnetSystemAdaptersTestMemory)
        .DependsOn(DotnetSystemAdaptersTestSqlClient)
        .DependsOn(DotnetSystemAdaptersTestNpgsql);
}
