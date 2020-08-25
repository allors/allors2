namespace Allors.Meta
{
    public partial class MetaPerson
    {
        internal override void CustomExtend()
        {
            this.Delete.Workspace = true;
        }
    }
}