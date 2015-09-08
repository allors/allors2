namespace Allors.Meta
{
    public partial class PurchaseOrderClass
	{
	    internal override void AppsExtend()
	    {
	        this.CurrentObjectState.RoleType.IsRequired = true;
            this.ShipToBuyer.RoleType.IsRequired = true;
            this.BillToPurchaser.RoleType.IsRequired = true;
            this.TakenViaSupplier.RoleType.IsRequired = true;
            this.BillToContactMechanism.RoleType.IsRequired = true;
        }
	}
}