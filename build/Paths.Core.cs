using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Core => Root / "core";
    public AbsolutePath CoreRepositoryDomainRepository => Core / "Repository/Domain/Repository.csproj";
    public AbsolutePath CoreDatabaseMetaGenerated => Core / "Database/Meta/generated";
    public AbsolutePath CoreDatabaseGenerate => Core / "Database/Generate/Generate.csproj";
}
