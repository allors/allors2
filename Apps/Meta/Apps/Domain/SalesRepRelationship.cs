namespace Allors.Meta
{
    public partial class SalesRepRelationshipClass
	{
	    internal override void AppsExtend()
        {
			this.LastYearsCommission.RoleType.IsRequired = true;
			this.YTDCommission.RoleType.IsRequired = true;

            this.Customer.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
            this.SalesRepresentative.RoleType.IsRequired = true;
		}
	}
}