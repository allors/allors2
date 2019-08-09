using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Artifacts => Root / "artifacts";

    public AbsolutePath ArtifactsTests => Artifacts / "Tests";

    public AbsolutePath ArtifactsTestsCoreWorkspaceTypescriptDomain => ArtifactsTests / "CoreWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsTestsCoreWorkspaceTypescriptPromise => ArtifactsTests / "CoreWorkspaceTypescriptPromise.trx";
    public AbsolutePath ArtifactsCoreCommands => Artifacts / "Core/Commands";
    public AbsolutePath ArtifactsCoreServer => Artifacts / "Core/Server";

    public AbsolutePath ArtifactsTestsAppsWorkspaceTypescriptDomain => ArtifactsTests / "AppsWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsAppsCommands => Artifacts / "Apps/Commands";
    public AbsolutePath ArtifactsAppsServer => Artifacts / "Apps/Server";
    public AbsolutePath ArtifactsAppsExcellAddIn => Artifacts / "Apps/ExcelAddIn";
}
