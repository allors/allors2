namespace Allors.Meta
{
    public partial class PartyPackageRevenueClass
	{
	    internal override void AppsExtend()
        {
            this.Year.RoleType.IsRequired = true;
            this.Month.RoleType.IsRequired = true;
            this.Revenue.RoleType.IsRequired = true;
        }
	}
}