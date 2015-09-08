namespace Allors.Meta
{
    public partial class PartyProductCategoryRevenueHistoryClass
	{
	    internal override void AppsExtend()
	    {
	        this.Quantity.RoleType.IsRequired = true;
            this.Revenue.RoleType.IsRequired = true;
        }
	}
}