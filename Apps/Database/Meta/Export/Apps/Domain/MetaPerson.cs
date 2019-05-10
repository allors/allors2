namespace Allors.Meta
{
    public partial class MetaPerson
    {
        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;
        }
    }
}