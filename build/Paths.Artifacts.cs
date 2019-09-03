using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public AbsolutePath Artifacts => Root / "artifacts";

    public AbsolutePath ArtifactsTests => Artifacts / "Tests";

    public AbsolutePath ArtifactsTestsCoreWorkspaceTypescriptDomain => ArtifactsTests / "CoreWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsTestsCoreWorkspaceTypescriptPromise => ArtifactsTests / "CoreWorkspaceTypescriptPromise.trx";
    public AbsolutePath ArtifactsCoreCommands => Artifacts / "Core/Commands";
    public AbsolutePath ArtifactsCoreApi => Artifacts / "Core/Api";

    public AbsolutePath ArtifactsTestsBaseWorkspaceTypescriptDomain => ArtifactsTests / "BaseWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsBaseCommands => Artifacts / "Base/Commands";
    public AbsolutePath ArtifactsBaseApi => Artifacts / "Base/Api";
    public AbsolutePath ArtifactsBaseExcellAddIn => Artifacts / "Base/ExcelAddIn";
}
