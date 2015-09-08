namespace Allors.Meta
{
    public partial class SalesInvoiceItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.SalesInvoiceItemObjectState.RoleType.IsRequired = true;
        }
	}
}