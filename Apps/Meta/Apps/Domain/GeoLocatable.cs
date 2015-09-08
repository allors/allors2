namespace Allors.Meta
{
    public partial class GeoLocatableInterface
	{
	    internal override void AppsExtend()
        {
			this.Latitude.RoleType.IsRequired = true;
			this.Longitude.RoleType.IsRequired = true;

		}
	}
}