namespace Allors.Meta
{
    public partial class WorkEffortInventoryAssignmentClass
	{
	    internal override void AppsExtend()
        {
            this.Assignment.RoleType.IsRequired = true;
            this.InventoryItem.RoleType.IsRequired = true;
        }
	}
}