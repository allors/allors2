namespace Allors.Meta
{
    public partial class MetaOrganisation
    {
        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;
        }
    }
}