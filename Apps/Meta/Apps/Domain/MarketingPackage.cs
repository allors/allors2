namespace Allors.Meta
{
    public partial class MarketingPackageClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}