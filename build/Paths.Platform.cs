using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Platform => Root / "platform";
    public AbsolutePath PlatformRepositoryTemplates => Platform / "Repository/Templates";
    public AbsolutePath PlatformRepositoryTemplatesMetaCs => PlatformRepositoryTemplates / "meta.cs.stg";
    public AbsolutePath PlatformRepositoryGenerate => Platform / "Repository/Generate/Generate.csproj";

    public AbsolutePath PlatformDatabase => Platform / "Database";

    public AbsolutePath PlatformAdapters => PlatformDatabase / "Adapters";
    public AbsolutePath PlatformAdaptersRepositoryDomainRepository => PlatformAdapters / "Repository/Domain/Repository.csproj";
    public AbsolutePath PlatformAdaptersMetaGenerated => PlatformAdapters / "Meta/generated";
    public AbsolutePath PlatformAdaptersGenerate => PlatformAdapters / "Generate/Generate.csproj";
    public AbsolutePath PlatformAdaptersStaticTests => PlatformAdapters / "Tests.Static/Tests.Static.csproj";
}
