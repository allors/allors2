using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public Paths(AbsolutePath root)
    {
        Root = root;

        Artifacts = Root / "artifacts";
        ArtifactsTests = Artifacts / "Tests";
    }

    public AbsolutePath Root { get; }

    public AbsolutePath Artifacts { get; }
    public AbsolutePath ArtifactsTests { get; }
}
