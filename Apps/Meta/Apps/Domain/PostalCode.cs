namespace Allors.Meta
{
    public partial class PostalCodeClass
	{
	    internal override void AppsExtend()
        {
            this.Code.RoleType.IsRequired = true;
        }
	}
}