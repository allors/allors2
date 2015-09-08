namespace Allors.Meta
{
    public partial class RequirementStatusClass
	{
	    internal override void AppsExtend()
        {
            this.StartDateTime.RoleType.IsRequired = true;
            this.RequirementObjectState.RoleType.IsRequired = true;
        }
	}
}