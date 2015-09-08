namespace Allors.Meta
{
    public partial class SalesInvoiceStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.SalesInvoiceObjectState.RoleType.IsRequired = true;
        }
	}
}