namespace Allors.Meta
{
    public partial class PackagingContentClass
	{
	    internal override void AppsExtend()
        {
            this.Quantity.RoleType.IsRequired = true;
            this.ShipmentItem.RoleType.IsRequired = true;
        }
	}
}