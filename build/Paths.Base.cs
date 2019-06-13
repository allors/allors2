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
    public AbsolutePath BaseWorkspaceTypescriptMaterial => Base / "Workspace/Typescript/Material";
    public AbsolutePath BaseWorkspaceTypescriptAutotestAngular => Base / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath[] BaseWorkspaceTypescript => new[]
    {
        BaseWorkspaceTypescriptDomain,
        BaseWorkspaceTypescriptPromise,
        BaseWorkspaceTypescriptAngular,
        BaseWorkspaceTypescriptMaterial,
        BaseWorkspaceTypescriptAutotestAngular
    };
    public AbsolutePath BaseWorkspaceTypescriptAutotestGenerateGenerate => Base / "Workspace/Typescript/Autotest/Generate/Generate.csproj";

}
