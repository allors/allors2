using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Base => Root / "base";
    public AbsolutePath BaseRepositoryDomainRepository => Base / "Repository/Domain/Repository.csproj";
    public AbsolutePath BaseDatabaseMetaGenerated => Base / "Database/Meta/generated";
    public AbsolutePath BaseDatabaseGenerate => Base / "Database/Generate/Generate.csproj";
    public AbsolutePath BaseDatabaseServer => Base / "Database/Server";
    public AbsolutePath BaseDatabaseServerProject => BaseDatabaseServer / "Server.csproj";
    public AbsolutePath BaseDatabaseDomainTests => Base / "Database/Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath BaseDatabaseServerTests => Base / "Database/Server.Tests/Server.Tests.csproj";

    public AbsolutePath BaseWorkspaceTypescriptDomain => Base / "Workspace/Typescript/Domain";
    public AbsolutePath BaseWorkspaceTypescriptPromise => Base / "Workspace/Typescript/Promise";
    public AbsolutePath BaseWorkspaceTypescriptAngular => Base / "Workspace/Typescript/Angular";
    public AbsolutePath BaseWorkspaceTypescriptAngularTrx => BaseWorkspaceTypescriptAngular / "dist/BaseWorkspaceTypescriptAngular.trx";
    public AbsolutePath BaseWorkspaceTypescriptMaterial => Base / "Workspace/Typescript/Material";
    public AbsolutePath BaseWorkspaceTypescriptMaterialTrx => BaseWorkspaceTypescriptMaterial / "dist/BaseWorkspaceTypescriptMaterial.trx";
    public AbsolutePath BaseWorkspaceTypescriptMaterialTests => Base / "Workspace/Typescript/Material.Tests/Material.Tests.csproj";
    public AbsolutePath BaseWorkspaceTypescriptAutotestAngular => Base / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath BaseWorkspaceTypescriptAutotestGenerateGenerate => Base / "Workspace/Typescript/Autotest/Generate/Generate.csproj";
    public AbsolutePath[] BaseWorkspaceTypescript => new[]
    {
        BaseWorkspaceTypescriptDomain,
        BaseWorkspaceTypescriptPromise,
        BaseWorkspaceTypescriptAngular,
        BaseWorkspaceTypescriptMaterial,
        BaseWorkspaceTypescriptAutotestAngular
    };
}
