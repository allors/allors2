namespace Allors.Meta
{
    public partial class MetaPeriod
    {
        internal override void AppsExtend()
        {
            this.FromDate.RelationType.Workspace = true;
            this.ThroughDate.RelationType.Workspace = true;
        }
    }
}