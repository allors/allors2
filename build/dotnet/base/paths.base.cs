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

    public AbsolutePath BaseWorkspaceTypescript => Base / "Workspace/Typescript";
    public AbsolutePath BaseWorkspaceIntranetTests => Base / "Workspace/Scaffold/Intranet.Tests";
    public AbsolutePath BaseWorkspaceScaffoldGenerate => Base / "Workspace/Scaffold/Generate/Generate.csproj";
}
