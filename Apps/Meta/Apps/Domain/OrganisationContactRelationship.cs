namespace Allors.Meta
{
    public partial class OrganisationContactRelationshipClass
	{
	    internal override void AppsExtend()
        {
            this.Contact.RoleType.IsRequired = true;
            this.Organisation.RoleType.IsRequired = true;
        }
	}
}