namespace Allors.Meta
{
    public partial class OrderKindClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.ScheduleManually.RoleType.IsRequired = true;
        }
	}
}