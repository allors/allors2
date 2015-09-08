namespace Allors.Meta
{
    public partial class ItemIssuanceClass
	{
	    internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.InventoryItem.RoleType.IsRequired = true;
            this.ShipmentItem.RoleType.IsRequired = true;
        }
	}
}