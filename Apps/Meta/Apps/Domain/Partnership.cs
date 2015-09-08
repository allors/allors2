namespace Allors.Meta
{
    public partial class PartnershipClass
	{
	    internal override void AppsExtend()
        {
            this.Partner.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
		}
	}
}