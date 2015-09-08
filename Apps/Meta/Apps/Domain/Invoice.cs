namespace Allors.Meta
{
    public partial class InvoiceInterface
	{
	    internal override void AppsExtend()
        {
			this.TotalFeeCustomerCurrency.RoleType.IsRequired = true;
			this.TotalExVatCustomerCurrency.RoleType.IsRequired = true;
			this.AmountPaid.RoleType.IsRequired = true;
			this.TotalDiscount.RoleType.IsRequired = true;
			this.TotalIncVat.RoleType.IsRequired = true;
			this.TotalSurcharge.RoleType.IsRequired = true;
			this.TotalBasePrice.RoleType.IsRequired = true;
			this.TotalVatCustomerCurrency.RoleType.IsRequired = true;
			this.EntryDate.RoleType.IsRequired = true;
			this.TotalIncVatCustomerCurrency.RoleType.IsRequired = true;
			this.TotalShippingAndHandling.RoleType.IsRequired = true;
			this.TotalBasePriceCustomerCurrency.RoleType.IsRequired = true;
			this.TotalExVat.RoleType.IsRequired = true;
			this.TotalSurchargeCustomerCurrency.RoleType.IsRequired = true;
			this.TotalDiscountCustomerCurrency.RoleType.IsRequired = true;
			this.TotalVat.RoleType.IsRequired = true;
			this.TotalFee.RoleType.IsRequired = true;
            this.InvoiceDate.RoleType.IsRequired = true;
            this.InvoiceNumber.RoleType.IsRequired = true;
            this.TotalShippingAndHandlingCustomerCurrency.RoleType.IsRequired = true;
		}
	}
}