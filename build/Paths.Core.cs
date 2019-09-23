using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Core => Root / "core";
    public AbsolutePath CoreRepositoryDomainRepository => Core / "Repository/Domain/Repository.csproj";
    public AbsolutePath CoreDatabaseMetaGenerated => Core / "Database/Meta/generated";
    public AbsolutePath CoreDatabaseGenerate => Core / "Database/Generate/Generate.csproj";
    public AbsolutePath CoreDatabaseServer => Core / "Database/Server";
    public AbsolutePath CoreDatabaseCommands => Core / "Database/Commands";
    public AbsolutePath CoreDatabaseDomainTests => Core / "Database/Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath CoreDatabaseServerTests => Core / "Database/Server.Tests/Server.Tests.csproj";

    public AbsolutePath CoreWorkspaceTypescriptDomain => Core / "Workspace/Typescript/Domain";
    public AbsolutePath CoreWorkspaceTypescriptPromise => Core / "Workspace/Typescript/Promise";
    public AbsolutePath CoreWorkspaceTypescriptAngular => Core / "Workspace/Typescript/Angular";
    public AbsolutePath CoreWorkspaceTypescriptAngularTrx => CoreWorkspaceTypescriptAngular / "dist/CoreWorkspaceTypescriptAngular.trx";
    public AbsolutePath CoreWorkspaceTypescriptMaterial => Core / "Workspace/Typescript/Material";
    public AbsolutePath CoreWorkspaceTypescriptMaterialTrx => CoreWorkspaceTypescriptMaterial / "dist/CoreWorkspaceTypescriptMaterial.trx";
    public AbsolutePath CoreWorkspaceTypescriptMaterialTests => Core / "Workspace/Typescript/Material.Tests/Material.Tests.csproj";
    public AbsolutePath CoreWorkspaceTypescriptAutotestAngular => Core / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath CoreWorkspaceTypescriptAutotestGenerateGenerate => Core / "Workspace/Typescript/Autotest/Generate/Generate.csproj";
    public AbsolutePath[] CoreWorkspaceTypescript => new[]
    {
        CoreWorkspaceTypescriptDomain,
        CoreWorkspaceTypescriptPromise,
        CoreWorkspaceTypescriptAngular,
        CoreWorkspaceTypescriptMaterial,
        CoreWorkspaceTypescriptAutotestAngular
    };

    public AbsolutePath CoreWorkspaceCSharpDomainTests => Core / "Workspace/CSharp/Domain.Tests";

}
