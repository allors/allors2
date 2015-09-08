namespace Allors.Meta
{
    public partial class PurchaseOrderItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PurchaseOrderItemObjectState.RoleType.IsRequired = true;
        }
	}
}