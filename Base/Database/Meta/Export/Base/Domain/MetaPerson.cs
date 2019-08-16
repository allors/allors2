namespace Allors.Meta
{
    public partial class MetaPerson
    {
        internal override void BaseExtend() => this.Delete.Workspace = true;
    }
}
