namespace Allors.Meta
{
    public partial class OrderShipmentClass
	{
	    internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.Picked.RoleType.IsRequired = true;
            this.ShipmentItem.RoleType.IsRequired = true;
        }
	}
}