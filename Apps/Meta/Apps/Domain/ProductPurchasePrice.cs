namespace Allors.Meta
{
    public partial class ProductPurchasePriceClass
	{
	    internal override void AppsExtend()
        {
            this.Price.RoleType.IsRequired = true;
            this.Currency.RoleType.IsRequired = true;
            this.UnitOfMeasure.RoleType.IsRequired = true;
        }
	}
}