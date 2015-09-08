namespace Allors.Meta
{
    public partial class SupplierOfferingClass
	{
	    internal override void AppsExtend()
        {
            this.Supplier.RoleType.IsRequired = true;
            this.ProductPurchasePrice.RoleType.IsRequired = true;
        }
	}
}