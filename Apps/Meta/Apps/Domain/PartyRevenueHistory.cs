namespace Allors.Meta
{
    public partial class PartyRevenueHistoryClass
	{
	    internal override void AppsExtend()
	    {
	        this.Revenue.RoleType.IsRequired = true;
	    }
	}
}