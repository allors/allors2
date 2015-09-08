namespace Allors.Meta
{
    public partial class SalesAccountingTransactionClass
	{
	    internal override void AppsExtend()
        {
            this.Invoice.RoleType.IsRequired = true;
        }
	}
}