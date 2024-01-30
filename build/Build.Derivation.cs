using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build
{
    Target DerivationGenerate => _ => _
        .After(Clean)
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.PlatformRepositoryGenerate)
                .SetApplicationArguments($"{Paths.DerivationRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.DerivationDatabaseMetaGenerated}"));
            DotNetRun(s => s
                .SetProcessWorkingDirectory(Paths.Derivation)
                .SetProjectFile(Paths.DerivationDatabaseGenerate));
        });

    Target DerivationTest => _ => _
        .DependsOn(DerivationGenerate)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Paths.DerivationDatabaseDomainTests)
                .AddLoggers("trx;LogFileName=next.trx")
                .SetResultsDirectory(Paths.ArtifactsTests));
        });

    Target Derivation => _ => _
        .After(Clean)
        .DependsOn(DerivationTest);
}
