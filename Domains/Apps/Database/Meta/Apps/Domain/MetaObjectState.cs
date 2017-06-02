namespace Allors.Meta
{
    public partial class MetaObjectState
    {
        internal override void AppsExtend()
        {
            this.Name.RelationType.Workspace = true;
        }
    }
}