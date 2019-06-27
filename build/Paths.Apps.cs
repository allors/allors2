using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Apps => Root / "apps";
    public AbsolutePath AppsRepositoryDomainRepository => Apps / "Repository/Domain/Repository.csproj";
    public AbsolutePath AppsDatabaseMetaGenerated => Apps / "Database/Meta/generated";
    public AbsolutePath AppsDatabaseGenerate => Apps / "Database/Generate/Generate.csproj";
    public AbsolutePath AppsDatabaseCommands => Apps / "Database/Commands";
    public AbsolutePath AppsDatabaseServer => Apps / "Database/Server";
    public AbsolutePath AppsDatabaseDomainTests => Apps / "Database/Domain.Tests/Domain.Tests.csproj";

    public AbsolutePath AppsWorkspaceCSharp => Apps / "Workspace/CSharp";
    public AbsolutePath AppsWorkspaceCSharpExcelAddIn => AppsWorkspaceCSharp / "ExcelAddIn";
    public AbsolutePath AppsWorkspaceCSharpExcelAddInProject => AppsWorkspaceCSharpExcelAddIn / "ExcelAddIn.csproj";
    public AbsolutePath AppsWorkspaceCSharpExcelAddInSignTool => AppsWorkspaceCSharpExcelAddIn / "signtool.exe";

    public AbsolutePath AppsWorkspaceTypescriptDomain => Apps / "Workspace/Typescript/Domain";
    public AbsolutePath AppsWorkspaceTypescriptIntranet => Apps / "Workspace/Typescript/Intranet";
    public AbsolutePath AppsWorkspaceTypescriptIntranetTrx => AppsWorkspaceTypescriptIntranet / "dist/AppsWorkspaceTypescriptIntranet.trx";
    public AbsolutePath AppsWorkspaceTypescriptIntranetTests => Apps / "Workspace/Typescript/Intranet.Tests/Intranet.Tests.csproj";
    public AbsolutePath AppsWorkspaceTypescriptAutotestAngular => Apps / "Workspace/Typescript/Autotest/Angular";
    public AbsolutePath AppsWorkspaceTypescriptAutotestGenerateGenerate => Apps / "Workspace/Typescript/Autotest/Generate/Generate.csproj";
    public AbsolutePath[] AppsWorkspaceTypescript => new[]
    {
        AppsWorkspaceTypescriptDomain,
        AppsWorkspaceTypescriptIntranet,
        AppsWorkspaceTypescriptAutotestAngular
    };
}
