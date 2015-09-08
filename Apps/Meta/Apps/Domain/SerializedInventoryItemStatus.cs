namespace Allors.Meta
{
    public partial class SerializedInventoryItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.SerializedInventoryItemObjectState.RoleType.IsRequired = true;
        }
	}
}