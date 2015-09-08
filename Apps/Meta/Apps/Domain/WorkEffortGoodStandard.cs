namespace Allors.Meta
{
    public partial class WorkEffortGoodStandardClass
	{
	    internal override void AppsExtend()
        {
            this.Good.RoleType.IsRequired = true;
        }
	}
}