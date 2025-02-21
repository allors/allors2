using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Core => Root / "Core";
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

    public AbsolutePath CoreWorkspaceTypescript=> Core / "Workspace/Typescript";

    public AbsolutePath CoreWorkspaceCSharpDomainTests => Core / "Workspace/CSharp/Domain.Tests";
}
