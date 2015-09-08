namespace Allors.Meta
{
    public partial class PostalAddressClass
	{
	    internal override void AppsExtend()
        {
			this.FormattedFullAddress.RoleType.IsRequired = true;

            this.Address1.RoleType.IsRequired = true;
		}
	}
}