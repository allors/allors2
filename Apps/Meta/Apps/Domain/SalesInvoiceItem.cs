namespace Allors.Meta
{
	public partial class SalesInvoiceItemClass
	{
	    internal override void AppsExtend()
	    {
	        this.CurrentObjectState.RoleType.IsRequired = true;

			this.InitialMarkupPercentage.RoleType.IsRequired = true;
			this.MaintainedMarkupPercentage.RoleType.IsRequired = true;
			this.UnitPurchasePrice.RoleType.IsRequired = true;
			this.InitialProfitMargin.RoleType.IsRequired = true;
			this.MaintainedProfitMargin.RoleType.IsRequired = true;

            this.SalesInvoiceItemType.RoleType.IsRequired = true;

            this.ConcreteRoleTypeByRoleType[InvoiceItemInterface.Instance.Quantity.RoleType].IsRequiredOverride = true;
            this.ConcreteRoleTypeByRoleType[InvoiceItemInterface.Instance.AmountPaid.RoleType].IsRequiredOverride = true;
		}
	}
}