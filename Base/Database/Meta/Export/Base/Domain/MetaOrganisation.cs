namespace Allors.Meta
{
    public partial class MetaOrganisation
    {
        internal override void BaseExtend()
        {
            this.Delete.Workspace = true;
        }
    }
}