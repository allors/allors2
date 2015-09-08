namespace Allors.Meta
{
    public partial class PickListStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.PickListObjectState.RoleType.IsRequired = true;
        }
	}
}