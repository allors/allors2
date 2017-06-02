namespace Allors.Meta
{
    public partial class MetaEnumeration
    {
        internal override void AppsExtend()
        {
            this.Name.RelationType.Workspace = true;
            this.IsActive.RelationType.Workspace = true;
        }
    }
}