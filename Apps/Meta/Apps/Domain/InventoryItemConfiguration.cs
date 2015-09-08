namespace Allors.Meta
{
    public partial class InventoryItemConfigurationInterface
	{
	    internal override void AppsExtend()
        {
            this.ComponentInventoryItem.RoleType.IsRequired = true;
            this.InventoryItem.RoleType.IsRequired = true;
            this.Quantity.RoleType.IsRequired = true;
        }
	}
}