namespace Allors.Meta
{
    public partial class WorkEffortAssignmentRateClass
	{
	    internal override void AppsExtend()
        {
            this.RateType.RoleType.IsRequired = true;
            this.WorkEffortPartyAssignment.RoleType.IsRequired = true;
        }
	}
}