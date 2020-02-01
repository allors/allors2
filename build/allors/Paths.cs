using Nuke.Common.IO;
using AbsolutePath = Nuke.Common.IO.PathConstruction.AbsolutePath;

public partial class Paths
{
    public Paths(AbsolutePath root)
    {
        Root = root;
    }

    public PathConstruction.AbsolutePath Root { get; }

    public PathConstruction.AbsolutePath SignTool = (PathConstruction.AbsolutePath)@"C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe";
}
