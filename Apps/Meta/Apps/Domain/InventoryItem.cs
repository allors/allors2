namespace Allors.Meta
{
    public partial class InventoryItemInterface
	{
	    internal override void AppsExtend()
        {
			this.Name.RoleType.IsRequired = true;
			this.Sku.RoleType.IsRequired = true;
            this.Facility.RoleType.IsRequired = true;
		}
	}
}