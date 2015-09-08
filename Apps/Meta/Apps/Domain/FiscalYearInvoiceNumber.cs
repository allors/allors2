namespace Allors.Meta
{
    public partial class FiscalYearInvoiceNumberClass
	{
	    internal override void AppsExtend()
        {
			this.NextSalesInvoiceNumber.RoleType.IsRequired = true;

            this.FiscalYear.RoleType.IsRequired = true;
		}
	}
}