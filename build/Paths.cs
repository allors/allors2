using static Nuke.Common.IO.PathConstruction;

public partial class Paths
{
    public Paths(AbsolutePath root)
    {
        Root = root;
    }

    public AbsolutePath Root { get; }
}
