namespace Allors.Meta
{
    public partial class ShipmentReceiptClass
	{
	    internal override void AppsExtend()
        {
            this.ReceivedDateTime.RoleType.IsRequired = true;
            this.InventoryItem.RoleType.IsRequired = true;
            this.QuantityAccepted.RoleType.IsRequired = true;
            this.QuantityRejected.RoleType.IsRequired = true;
            this.ShipmentItem.RoleType.IsRequired = true;
        }
	}
}