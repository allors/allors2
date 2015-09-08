namespace Allors.Meta
{
    public partial class PackageClass
	{
	    internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
        }
	}
}