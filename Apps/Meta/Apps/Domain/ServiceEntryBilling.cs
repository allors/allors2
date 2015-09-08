namespace Allors.Meta
{
    public partial class ServiceEntryBillingClass
	{
	    internal override void AppsExtend()
        {
            this.InvoiceItem.RoleType.IsRequired = true;
            this.ServiceEntry.RoleType.IsRequired = true;
        }
	}
}