namespace Allors.Meta
{
    public partial class PurchaseInvoiceItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PurchaseInvoiceItemObjectState.RoleType.IsRequired = true;
        }
	}
}