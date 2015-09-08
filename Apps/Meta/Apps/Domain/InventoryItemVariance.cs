namespace Allors.Meta
{
    public partial class InventoryItemVarianceClass
	{
	    internal override void AppsExtend()
        {
            this.Reason.RoleType.IsRequired = true;
            this.Quantity.RoleType.IsRequired = true;
        }
	}
}