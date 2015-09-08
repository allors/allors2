namespace Allors.Meta
{
    public partial class OrderItemBillingClass
	{
	    internal override void AppsExtend()
        {
            this.OrderItem.RoleType.IsRequired = true;
            this.Amount.RoleType.IsRequired = true;
        }
	}
}