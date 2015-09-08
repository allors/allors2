namespace Allors.Meta
{
    public partial class PurchaseOrderStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PurchaseOrderObjectState.RoleType.IsRequired = true;
        }
	}
}