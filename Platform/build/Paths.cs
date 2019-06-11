using static Nuke.Common.IO.PathConstruction;

public class Paths
{
    public Paths(AbsolutePath root)
    {
        Platform = root;
        PlatformRepositoryTemplates = Platform / "Repository/Templates";
        PlatformRepositoryTemplatesMetaCs = PlatformRepositoryTemplates / "meta.cs.stg";
        PlatformRepositoryGenerate = Platform / "Repository/Generate/Generate.csproj";

        PlatformAdapters = Platform / "Adapters";
        PlatformAdaptersRepositoryDomainRepository = PlatformAdapters / "Repository/Domain/Repository.csproj";
        PlatformAdaptersMetaGenerated = PlatformAdapters / "Meta/generated";
        PlatformAdaptersGenerate = PlatformAdapters / "Generate/Generate.csproj";
        PlatformAdaptersStaticTests = PlatformAdapters / "Tests.Static/Tests.Static.csproj";

        Artifacts = Platform / "artifacts";
        ArtifactsTests = Artifacts / "Tests";
    }

    public AbsolutePath Platform { get; }
    public AbsolutePath PlatformRepositoryTemplates { get; }
    public AbsolutePath PlatformRepositoryTemplatesMetaCs { get; }
    public AbsolutePath PlatformRepositoryGenerate { get; }

    public AbsolutePath PlatformAdapters { get; }
    public AbsolutePath PlatformAdaptersRepositoryDomainRepository { get; }
    public AbsolutePath PlatformAdaptersMetaGenerated { get; }
    public AbsolutePath PlatformAdaptersGenerate { get; }
    public AbsolutePath PlatformAdaptersStaticTests { get; }

    public AbsolutePath Artifacts { get; }
    public AbsolutePath ArtifactsTests { get; }
}
