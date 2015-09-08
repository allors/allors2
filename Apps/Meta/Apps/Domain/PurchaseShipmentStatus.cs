namespace Allors.Meta
{
    public partial class PurchaseShipmentStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PurchaseShipmentObjectState.RoleType.IsRequired = true;
        }
	}
}