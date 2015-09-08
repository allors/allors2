namespace Allors.Meta
{
    public partial class PartyInterface
	{
	    internal override void AppsExtend()
        {
			this.YTDRevenue.RoleType.IsRequired = true;
			this.LastYearsRevenue.RoleType.IsRequired = true;
			this.PartyName.RoleType.IsRequired = true;
			this.OpenOrderAmount.RoleType.IsRequired = true;

		}
	}
}