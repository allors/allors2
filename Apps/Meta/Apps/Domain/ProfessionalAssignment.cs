namespace Allors.Meta
{
    public partial class ProfessionalAssignmentClass
	{
	    internal override void AppsExtend()
        {
            this.Professional.RoleType.IsRequired = true;
            this.EngagementItem.RoleType.IsRequired = true;
        }
	}
}