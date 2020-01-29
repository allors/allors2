using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Core => Root / "core";
    public AbsolutePath CoreRepositoryDomainRepository => Core / "Repository/Domain/Repository.csproj";

    public AbsolutePath CoreDatabase => Core / "Database";
    public AbsolutePath CoreDatabaseMetaGenerated => CoreDatabase / "Meta/generated";
    public AbsolutePath CoreDatabaseGenerate => CoreDatabase / "Generate/Generate.csproj";
    public AbsolutePath CoreDatabaseMerge => CoreDatabase / "Merge/Merge.csproj";
    public AbsolutePath CoreDatabaseServer => CoreDatabase / "Server";
    public AbsolutePath CoreDatabaseCommands => CoreDatabase / "Commands";
    public AbsolutePath CoreDatabaseDomainTests => CoreDatabase / "Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath CoreDatabaseServerTests => CoreDatabase / "Server.Tests/Server.Tests.csproj";
    public AbsolutePath CoreDatabaseResources => CoreDatabase / "Resources";
    public AbsolutePath CoreDatabaseResourcesCore => CoreDatabaseResources / "Core";
    public AbsolutePath CoreDatabaseResourcesCustom => CoreDatabaseResources / "Custom";

    public AbsolutePath CoreWorkspaceTypescriptDomain => Core / "Workspace/Typescript/Domain";
    public AbsolutePath CoreWorkspaceTypescriptPromise => Core / "Workspace/Typescript/Promise";
    public AbsolutePath CoreWorkspaceTypescriptAngular => Core / "Workspace/Typescript/Angular";
    public AbsolutePath CoreWorkspaceTypescriptAngularTrx => CoreWorkspaceTypescriptAngular / "dist/CoreWorkspaceTypescriptAngular.trx";
    public AbsolutePath CoreWorkspaceTypescriptMaterial => Core / "Workspace/Typescript/Material";
    public AbsolutePath CoreWorkspaceTypescriptMaterialTrx => CoreWorkspaceTypescriptMaterial / "dist/CoreWorkspaceTypescriptMaterial.trx";
    public AbsolutePath CoreWorkspaceTypescriptMaterialTests => Core / "Workspace/Typescript/Material.Tests/Material.Tests.csproj";
    public AbsolutePath CoreWorkspaceTypescriptAutotestAngular => Core / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath CoreWorkspaceTypescriptAutotestGenerateGenerate => Core / "Workspace/Typescript/Autotest/Generate/Generate.csproj";
    public AbsolutePath CoreWorkspaceTypescriptWebsite => Core / "Workspace/Typescript/Website";
    public AbsolutePath CoreWorkspaceTypescriptWebsitePluginsAllors => CoreWorkspaceTypescriptWebsite / "plugins/gatsby-source-allors";

    public AbsolutePath[] CoreWorkspaceTypescript => new[]
    {
        CoreWorkspaceTypescriptDomain,
        CoreWorkspaceTypescriptPromise,
        CoreWorkspaceTypescriptAngular,
        CoreWorkspaceTypescriptMaterial,
        CoreWorkspaceTypescriptAutotestAngular,
        CoreWorkspaceTypescriptWebsite,
        CoreWorkspaceTypescriptWebsitePluginsAllors,
    };

    public AbsolutePath CoreWorkspaceCSharpDomainTests => Core / "Workspace/CSharp/Domain.Tests";

}
