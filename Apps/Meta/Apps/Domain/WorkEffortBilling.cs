namespace Allors.Meta
{
    public partial class WorkEffortBillingClass
	{
	    internal override void AppsExtend()
	    {
	        this.InvoiceItem.RoleType.IsRequired = true;
            this.WorkEffort.RoleType.IsRequired = true;
        }
	}
}