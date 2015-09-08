namespace Allors.Meta
{
    public partial class ReceiptAccountingTransactionClass
	{
	    internal override void AppsExtend()
        {
            this.Receipt.RoleType.IsRequired = true;
        }
	}
}