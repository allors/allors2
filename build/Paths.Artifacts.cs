using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Artifacts => Root / "artifacts";

    public AbsolutePath ArtifactsTests => Artifacts / "Tests";

    public AbsolutePath ArtifactsTestsBaseWorkspaceTypescriptDomain => ArtifactsTests / "BaseWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsTestsBaseWorkspaceTypescriptPromise => ArtifactsTests / "BaseWorkspaceTypescriptPromise .trx";
    public AbsolutePath ArtifactsBaseServer => Artifacts / "Base/Server";

    public AbsolutePath ArtifactsTestsAppsWorkspaceTypescriptDomain => ArtifactsTests / "AppsWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsAppsCommands => Artifacts / "Apps/Commands";
    public AbsolutePath ArtifactsAppsServer => Artifacts / "Apps/Server";
}
