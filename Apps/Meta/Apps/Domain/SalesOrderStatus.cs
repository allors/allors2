namespace Allors.Meta
{
    public partial class SalesOrderStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.SalesOrderObjectState.RoleType.IsRequired = true;
        }
	}
}