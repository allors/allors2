namespace Allors.Meta
{
    public partial class PickListItemClass
	{
	    internal override void AppsExtend()
        {
            this.InventoryItem.RoleType.IsRequired = true;
            this.RequestedQuantity.RoleType.IsRequired = true;
        }
	}
}