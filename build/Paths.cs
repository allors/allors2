using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public Paths(AbsolutePath root)
    {
        Root = root;

        Artifacts = Root / "artifacts";
    }

    public AbsolutePath Root { get; }

    public AbsolutePath Artifacts { get; }
}
