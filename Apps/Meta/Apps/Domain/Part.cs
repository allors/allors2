namespace Allors.Meta
{
    public partial class PartInterface
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
            this.InventoryItemKind.RoleType.IsRequired = true;
            this.OwnedByParty.RoleType.IsRequired = true;
        }
	}
}