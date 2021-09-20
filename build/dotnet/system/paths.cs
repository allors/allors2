using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath DotnetSystem => Dotnet / "System";
    public AbsolutePath DotnetSystemRepositoryTemplates => DotnetSystem / "Repository/Templates";
    public AbsolutePath DotnetSystemRepositoryTemplatesMetaCs => DotnetSystemRepositoryTemplates / "meta.cs.stg";
    public AbsolutePath DotnetSystemRepositoryGenerate => DotnetSystem / "Repository/Generate/Generate.csproj";

    public AbsolutePath DotnetSystemDatabase => DotnetSystem / "Database";

    public AbsolutePath DotnetSystemAdapters => DotnetSystemDatabase / "Adapters";

    public AbsolutePath DotnetSystemAdaptersRepositoryDomainRepository => DotnetSystemAdapters / "Repository/Domain/Repository.csproj";

    public AbsolutePath DotnetSystemAdaptersMetaGenerated => DotnetSystemAdapters / "Meta/generated";
    public AbsolutePath DotnetSystemAdaptersGenerate => DotnetSystemAdapters / "Generate/Generate.csproj";
    public AbsolutePath DotnetSystemAdaptersStaticTests => DotnetSystemAdapters / "Tests.Static/Tests.Static.csproj";

    public AbsolutePath DotnetSystemWorkspaceTypescript => DotnetSystem / "Workspace/Typescript";
}
