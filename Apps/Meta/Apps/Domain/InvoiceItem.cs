namespace Allors.Meta
{
    public partial class InvoiceItemInterface
	{
	    internal override void AppsExtend()
        {
			this.TotalIncVatCustomerCurrency.RoleType.IsRequired = true;
			this.TotalVatCustomerCurrency.RoleType.IsRequired = true;
			this.TotalBasePrice.RoleType.IsRequired = true;
			this.TotalSurcharge.RoleType.IsRequired = true;
			this.TotalInvoiceAdjustment.RoleType.IsRequired = true;
			this.TotalExVatCustomerCurrency.RoleType.IsRequired = true;
			this.TotalDiscount.RoleType.IsRequired = true;
			this.CalculatedUnitPrice.RoleType.IsRequired = true;
			this.UnitDiscount.RoleType.IsRequired = true;
			this.TotalIncVat.RoleType.IsRequired = true;
			this.UnitBasePrice.RoleType.IsRequired = true;
			this.TotalSurchargeCustomerCurrency.RoleType.IsRequired = true;
			this.TotalInvoiceAdjustmentCustomerCurrency.RoleType.IsRequired = true;
			this.AmountPaid.RoleType.IsRequired = true;
			this.TotalDiscountCustomerCurrency.RoleType.IsRequired = true;
			this.UnitSurcharge.RoleType.IsRequired = true;
			this.TotalExVat.RoleType.IsRequired = true;
			this.TotalBasePriceCustomerCurrency.RoleType.IsRequired = true;
			this.TotalVat.RoleType.IsRequired = true;
			this.UnitVat.RoleType.IsRequired = true;

            this.Quantity.RoleType.IsRequired = true;
}
	}
}