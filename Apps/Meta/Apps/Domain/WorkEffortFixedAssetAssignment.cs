namespace Allors.Meta
{
    public partial class WorkEffortFixedAssetAssignmentClass
	{
	    internal override void AppsExtend()
        {
            this.FixedAsset.RoleType.IsRequired = true;
            this.Assignment.RoleType.IsRequired = true;
        }
	}
}