namespace Allors.Meta
{
    public partial class PostalBoundaryClass
	{
	    internal override void AppsExtend()
        {
            this.Locality.RoleType.IsRequired = true;
            this.Country.RoleType.IsRequired = true;
        }
	}
}