namespace Allors.Meta
{
    public partial class WorkEffortPartStandardClass
	{
	    internal override void AppsExtend()
        {
            this.Part.RoleType.IsRequired = true;
        }
	}
}