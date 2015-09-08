namespace Allors.Meta
{
    public partial class NonSerializedInventoryItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.NonSerializedInventoryItemObjectState.RoleType.IsRequired = true;
        }
	}
}