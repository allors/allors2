namespace Allors.Meta
{
    public partial class SalesOrderItemStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.SalesOrderItemObjectState.RoleType.IsRequired = true;
        }
	}
}