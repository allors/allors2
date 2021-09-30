using Nuke.Common.IO;

public partial class Paths
{
    public AbsolutePath Artifacts => Root / "artifacts";

    public AbsolutePath ArtifactsTests => Artifacts / "Tests";

    public AbsolutePath ArtifactsCoreCommands => Artifacts / "Core/Commands";
    public AbsolutePath ArtifactsCoreServer => Artifacts / "Core/Server";

    public AbsolutePath ArtifactsTestsBaseWorkspaceTypescriptDomain => ArtifactsTests / "BaseWorkspaceTypescriptDomain.trx";
    public AbsolutePath ArtifactsBaseCommands => Artifacts / "Base/Commands";
    public AbsolutePath ArtifactsBaseServer => Artifacts / "Base/Server";
    public AbsolutePath ArtifactsBaseExcellAddIn => Artifacts / "Base/ExcelAddIn";
}
