namespace Allors.Meta
{
    public partial class MetaOrganisationContactKind
    {
        internal override void AppsExtend()
        {
            this.Description.RelationType.Workspace = true;
        }
    }
}