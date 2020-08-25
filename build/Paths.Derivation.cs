using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Derivation => Root / "Demos/Derivation";
    public AbsolutePath DerivationRepositoryDomainRepository => Derivation / "Repository/Domain/Repository.csproj";
    public AbsolutePath DerivationDatabaseMetaGenerated => Derivation / "Database/Meta/generated";
    public AbsolutePath DerivationDatabaseGenerate => Derivation / "Database/Generate/Generate.csproj";
    public AbsolutePath DerivationDatabaseDomainTests => Derivation / "Database/Domain.Tests/Domain.Tests.csproj";
}

