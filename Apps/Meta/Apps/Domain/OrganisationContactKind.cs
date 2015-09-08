namespace Allors.Meta
{
    public partial class OrganisationContactKindClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}