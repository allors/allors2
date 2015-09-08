namespace Allors.Meta
{
    public partial class PerformanceReviewClass
	{
	    internal override void AppsExtend()
        {
            this.Employee.RoleType.IsRequired = true;
        }
	}
}