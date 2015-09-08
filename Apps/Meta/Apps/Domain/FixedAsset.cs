namespace Allors.Meta
{
    public partial class FixedAssetInterface
	{
	    internal override void AppsExtend()
	    {
	        this.Name.RoleType.IsRequired = true;
	    }
	}
}