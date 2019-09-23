using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Base => Root / "base";
    public AbsolutePath BaseRepositoryDomainRepository => Base / "Repository/Domain/Repository.csproj";
    public AbsolutePath BaseDatabaseMetaGenerated => Base / "Database/Meta/generated";
    public AbsolutePath BaseDatabaseGenerate => Base / "Database/Generate/Generate.csproj";
    public AbsolutePath BaseDatabaseCommands => Base / "Database/Commands";
    public AbsolutePath BaseDatabaseServer => Base / "Database/Server";
    public AbsolutePath BaseDatabaseDomainTests => Base / "Database/Domain.Tests/Domain.Tests.csproj";

    public AbsolutePath BaseWorkspaceCSharp => Base / "Workspace/CSharp";
    public AbsolutePath BaseWorkspaceCSharpExcelAddIn => BaseWorkspaceCSharp / "ExcelAddIn";
    public AbsolutePath BaseWorkspaceCSharpExcelAddInProject => BaseWorkspaceCSharpExcelAddIn / "ExcelAddIn.csproj";
    public AbsolutePath BaseWorkspaceCSharpExcelAddInSignTool => BaseWorkspaceCSharpExcelAddIn / "signtool.exe";

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
