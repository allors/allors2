namespace Allors.Meta
{
    public partial class WorkEffortAssignmentClass
	{
	    internal override void AppsExtend()
        {
            this.Assignment.RoleType.IsRequired = true;
            this.Professional.RoleType.IsRequired = true;
        }
	}
}