using System.Threading.Tasks;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.Npm;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;

partial class Build
{
    Target BaseResetDatabase => _ => _
        .Executes(() =>
        {
            var database = "Base";
            using (var sqlServer = new SqlServer())
            {
                sqlServer.Restart();
                sqlServer.Drop(database);
                sqlServer.Create(database);
            }
        });

    private Target BaseDatabaseTest => _ => _
         .DependsOn(BaseDatabaseTestDomain);

    private Target BaseMerge => _ => _
        .Executes(() =>
        {
            DotNetRun(s => s
                .SetProjectFile(Paths.CoreDatabaseMerge)
                .SetApplicationArguments($"{Paths.CoreDatabaseResourcesCore} {Paths.BaseDatabaseResourcesBase} {Paths.BaseDatabaseResources}"));
        });

    private Target BaseDatabaseTestDomain => _ => _
         .DependsOn(BaseGenerate)
         .Executes(() =>
         {
             DotNetTest(s => s
                 .SetProjectFile(Paths.BaseDatabaseDomainTests)
                 .SetLogger("trx;LogFileName=BaseDatabaseDomain.trx")
                 .SetResultsDirectory(Paths.ArtifactsTests));
         });

    private Target BaseGenerate => _ => _
         .After(Clean)
         .DependsOn(BaseMerge)
         .Executes(() =>
         {
             DotNetRun(s => s
                 .SetProjectFile(Paths.PlatformRepositoryGenerate)
                 .SetApplicationArguments($"{Paths.BaseRepositoryDomainRepository} {Paths.PlatformRepositoryTemplatesMetaCs} {Paths.BaseDatabaseMetaGenerated}"));
             DotNetRun(s => s
                 .SetWorkingDirectory(Paths.Base)
                 .SetProjectFile(Paths.BaseDatabaseGenerate));
         });

    private Target BasePublishCommands => _ => _
         .DependsOn(BaseGenerate)
         .Executes(() =>
         {
             var dotNetPublishSettings = new DotNetPublishSettings()
                 .SetWorkingDirectory(Paths.BaseDatabaseCommands)
                 .SetOutput(Paths.ArtifactsBaseCommands);
             DotNetPublish(dotNetPublishSettings);
         });

    private Target BasePublishServer => _ => _
             .DependsOn(BaseGenerate)
         .Executes(() =>
         {
             var dotNetPublishSettings = new DotNetPublishSettings()
                 .SetWorkingDirectory(Paths.BaseDatabaseServer)
                 .SetOutput(Paths.ArtifactsBaseServer);
             DotNetPublish(dotNetPublishSettings);
         });

    private Target BaseSetup => _ => _
        .Executes(() =>
        {
            NpmInstall(s => s
                .SetEnvironmentVariable("npm_config_loglevel", "error")
                .SetWorkingDirectory(Paths.BaseWorkspaceTypescript));
        });

    private Target BaseWorkspaceScaffold => _ => _
         .DependsOn(BaseGenerate)
         .Executes(() =>
         {
             NpmRun(s => s
                 .SetEnvironmentVariable("npm_config_loglevel", "error")
                 .SetWorkingDirectory(Paths.BaseWorkspaceTypescript)
                 .SetCommand("scaffold"));

             DotNetRun(s => s
                 .SetWorkingDirectory(Paths.Base)
                 .SetProjectFile(Paths.BaseWorkspaceScaffoldGenerate));
         });

    private Target BaseWorkspaceTypescriptDomain => _ => _
         .DependsOn(BaseGenerate)
         .DependsOn(EnsureDirectories)
         .Executes(() =>
         {
             NpmRun(s => s
                 .SetEnvironmentVariable("npm_config_loglevel", "error")
                 .SetWorkingDirectory(Paths.BaseWorkspaceTypescript)
                 .SetCommand("domain:test"));
         });

    private async Task BaseWorkspaceIntranetTest(string category)
    {
        using (var sqlServer = new SqlServer())
        {
            sqlServer.Restart();
            sqlServer.Populate(Paths.ArtifactsBaseCommands);
            using (var server = new Server(Paths.ArtifactsBaseServer))
            {
                using (var angular = new Angular(Paths.BaseWorkspaceTypescript, "intranet:serve"))
                {
                    await server.Ready();
                    await angular.Init();
                    DotNetTest(
                        s => s
                            .SetProjectFile(Paths.BaseWorkspaceIntranetTests)
                            .SetLogger($"trx;LogFileName=BaseIntranet{category}Tests.trx")
                            .SetFilter($"Category={category}")
                            .SetResultsDirectory(Paths.ArtifactsTests));
                }
            }
        }
    }

    private Target BaseWorkspaceIntranetGenericTests => _ => _
         .DependsOn(this.BaseWorkspaceScaffold)
         .DependsOn(this.BasePublishServer)
         .DependsOn(this.BasePublishCommands)
         .DependsOn(this.BaseResetDatabase)
         .Executes(async () =>
         {
             await this.BaseWorkspaceIntranetTest("Generic");
         });
    
    private Target BaseWorkspaceIntranetOtherTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("Other");
        });

    private Target BaseWorkspaceIntranetRelationTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("Relation");
        });

    private Target BaseWorkspaceIntranetInvoiceTests => _ => _
    .DependsOn(this.BaseWorkspaceScaffold)
    .DependsOn(BasePublishServer)
    .DependsOn(BasePublishCommands)
    .DependsOn(BaseResetDatabase)
    .Executes(async () =>
    {
        await this.BaseWorkspaceIntranetTest("Invoice");
    });

    private Target BaseWorkspaceIntranetOrderTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("Order");
        });

    private Target BaseWorkspaceIntranetProductTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("Product");
        });

    private Target BaseWorkspaceIntranetShipmentTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("Shipment");
        });

    private Target BaseWorkspaceIntranetWorkEffortTests => _ => _
        .DependsOn(this.BaseWorkspaceScaffold)
        .DependsOn(BasePublishServer)
        .DependsOn(BasePublishCommands)
        .DependsOn(BaseResetDatabase)
        .Executes(async () =>
        {
            await this.BaseWorkspaceIntranetTest("WorkEffort");
        });

    private Target BaseWorkspaceTypescriptTest => _ => _
        .DependsOn(BaseWorkspaceTypescriptDomain);

    private Target BaseTest => _ => _
        .DependsOn(BaseDatabaseTest)
        .DependsOn(BaseWorkspaceTypescriptTest);

    private Target Base => _ => _
        .DependsOn(Clean)
        .DependsOn(BaseTest);
}
