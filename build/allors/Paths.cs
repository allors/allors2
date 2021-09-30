using Nuke.Common.IO;

public partial class Paths
{
    public Paths(AbsolutePath root) => Root = root;

    public AbsolutePath Root { get; }

    public AbsolutePath SignTool =>
        (AbsolutePath)@"C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe";
}
