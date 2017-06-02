namespace Allors.Meta
{
    public partial class MetaLocale
    {
        internal override void AppsExtend()
        {
            this.Name.RelationType.Workspace = true;
        }
    }
}