using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Base => Root / "base";
    public AbsolutePath BaseRepositoryDomainRepository => Base / "Repository/Domain/Repository.csproj";

    public AbsolutePath BaseDatabase => Base / "Database";
    public AbsolutePath BaseDatabaseMetaGenerated => BaseDatabase / "Meta/generated";
    public AbsolutePath BaseDatabaseGenerate => BaseDatabase / "Generate/Generate.csproj";
    public AbsolutePath BaseDatabaseMerge => BaseDatabase / "Merge/Merge.csproj";
    public AbsolutePath BaseDatabaseCommands => BaseDatabase / "Commands";
    public AbsolutePath BaseDatabaseServer => BaseDatabase / "Server";
    public AbsolutePath BaseDatabaseDomainTests => BaseDatabase / "Domain.Tests/Domain.Tests.csproj";
    public AbsolutePath BaseDatabaseResources => BaseDatabase / "Resources";
    public AbsolutePath BaseDatabaseResourcesBase => BaseDatabaseResources / "Base";

    public AbsolutePath BaseWorkspaceCSharp => Base / "Workspace/CSharp";

    public AbsolutePath BaseWorkspaceTypescriptDomain => Base / "Workspace/Typescript/Domain";
    public AbsolutePath BaseWorkspaceTypescriptIntranet => Base / "Workspace/Typescript/Intranet";
    public AbsolutePath BaseWorkspaceTypescriptIntranetTrx => BaseWorkspaceTypescriptIntranet / "dist/BaseWorkspaceTypescriptIntranet.trx";
    public AbsolutePath BaseWorkspaceTypescriptIntranetTests => Base / "Workspace/Typescript/Intranet.Tests/Intranet.Tests.csproj";
    public AbsolutePath BaseWorkspaceTypescriptAutotestAngular => Base / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath BaseWorkspaceTypescriptAutotestGenerateGenerate => Base / "Workspace/Typescript/Autotest/Generate/Generate.csproj";
    public AbsolutePath[] BaseWorkspaceTypescript => new[]
    {
        BaseWorkspaceTypescriptDomain,
        BaseWorkspaceTypescriptIntranet,
        BaseWorkspaceTypescriptAutotestAngular
    };
}
