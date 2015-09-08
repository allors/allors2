namespace Allors.Meta
{
    public partial class SalesRepCommissionClass
	{
	    internal override void AppsExtend()
	    {
	        this.Year.RoleType.IsRequired = true;
	    }
	}
}