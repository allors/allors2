namespace Allors.Meta
{
    public partial class WorkEffortFixedAssetStandardClass
	{
	    internal override void AppsExtend()
        {
            this.FixedAsset.RoleType.IsRequired = true;
        }
	}
}