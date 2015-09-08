namespace Allors.Meta
{
    public partial class ShipmentItemClass
	{
	    internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.QuantityShipped.RoleType.IsRequired = true;
		}
	}
}