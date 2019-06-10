using static Nuke.Common.IO.PathConstruction;

public class Paths
{
    public Paths(AbsolutePath root)
    {
        Root = root;

        Platform = Root / "platform";
        PlatformRepositoryTemplates = Platform / "Repository/Templates";
        PlatformRepositoryTemplatesMetaCs = PlatformRepositoryTemplates / "meta.cs.stg";
        PlatformRepositoryGenerateGenerate = Platform / "Repository/Generate/Generate.csproj";

        PlatformAdapters = Platform / "Adapters";
        PlatformAdaptersRepositoryDomainRepository = PlatformAdapters / "Repository/Domain/Repository.csproj";
        PlatformAdaptersMetaGenerated = PlatformAdapters / "Meta/generated";
        PlatformAdaptersGenerate = PlatformAdapters / "Generate/Generate.csproj";

        Core = Root / "core";
        CoreRepositoryDomainRepository = Core / "Repository/Domain/Repository.csproj";
        CoreDatabaseMetaGenerated = Core / "Database/Meta/generated";
        CoreDatabaseGenerate = Core / "Database/Generate/Generate.csproj";

        Base = Root / "base";
        BaseRepositoryDomainRepository = Base / "Repository/Domain/Repository.csproj";
        BaseDatabaseMetaGenerated = Base / "Database/Meta/generated";
        BaseDatabaseGenerate = Base / "Database/Generate/Generate.csproj";
        BaseWorkspaceTypescriptDomain = Base / "Workspace/Typescript/Domain";
        BaseWorkspaceTypescriptPromise = Base / "Workspace/Typescript/Promise";
        BaseWorkspaceTypescriptAngular = Base / "Workspace/Typescript/Angular";
        BaseWorkspaceTypescriptMaterial = Base / "Workspace/Typescript/Material";
        BaseWorkspaceTypescriptAutotestAngular = Base / "Workspace/Typescript/Autotest/Angular";
        BaseWorkspaceTypescript = new[]
        {
            BaseWorkspaceTypescriptDomain,
            BaseWorkspaceTypescriptPromise,
            BaseWorkspaceTypescriptAngular,
            BaseWorkspaceTypescriptMaterial,
            BaseWorkspaceTypescriptAutotestAngular
        };

        BaseWorkspaceTypescriptAutotestGenerateGenerate = Base / "Workspace/Typescript/Autotest/Generate/Generate.csproj";

        Apps = Root / "apps";

        ArtifactsDirectory = Root / "artifacts";
    }

    public AbsolutePath Root { get; }

    public AbsolutePath Platform { get; }
    public AbsolutePath PlatformRepositoryTemplates { get; }
    public AbsolutePath PlatformRepositoryTemplatesMetaCs { get; }
    public AbsolutePath PlatformRepositoryGenerateGenerate { get; }

    public AbsolutePath PlatformAdapters { get; }
    public AbsolutePath PlatformAdaptersRepositoryDomainRepository { get; }
    public AbsolutePath PlatformAdaptersMetaGenerated { get; }
    public AbsolutePath PlatformAdaptersGenerate { get; }

    public AbsolutePath Core { get; }
    public AbsolutePath CoreRepositoryDomainRepository { get; }
    public AbsolutePath CoreDatabaseMetaGenerated { get; }
    public AbsolutePath CoreDatabaseGenerate { get; }

    public AbsolutePath Base { get; }
    public AbsolutePath BaseRepositoryDomainRepository { get; }
    public AbsolutePath BaseDatabaseMetaGenerated { get; }
    public AbsolutePath BaseDatabaseGenerate { get; }
    public AbsolutePath BaseWorkspaceTypescriptDomain { get; }
    public AbsolutePath BaseWorkspaceTypescriptPromise { get; }
    public AbsolutePath BaseWorkspaceTypescriptAngular { get; }
    public AbsolutePath BaseWorkspaceTypescriptMaterial { get; }
    public AbsolutePath BaseWorkspaceTypescriptAutotestAngular { get; }
    public AbsolutePath[] BaseWorkspaceTypescript { get; }
    public AbsolutePath BaseWorkspaceTypescriptAutotestGenerateGenerate { get; }

    public AbsolutePath Apps { get; }

    public AbsolutePath ArtifactsDirectory { get; }
}
