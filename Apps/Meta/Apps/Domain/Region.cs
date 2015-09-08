namespace Allors.Meta
{
    public partial class RegionClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}