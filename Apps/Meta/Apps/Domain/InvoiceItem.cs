namespace Allors.Meta
{
    public partial class InvoiceItemInterface
    {
        internal override void AppsExtend()
        {
            this.TotalInvoiceAdjustment.RoleType.IsRequired = true;
            this.TotalInvoiceAdjustmentCustomerCurrency.RoleType.IsRequired = true;
            this.AmountPaid.RoleType.IsRequired = true;
            this.Quantity.RoleType.IsRequired = true;
        }
    }
}