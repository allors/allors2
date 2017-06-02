namespace Allors.Meta
{
    public partial class MetaOrganisationContactRelationship
    {
        internal override void AppsExtend()
        {
            this.Organisation.RelationType.Workspace = true;
            this.Contact.RelationType.Workspace = true;
            this.ContactKinds.RelationType.Workspace = true;
        }
    }
}