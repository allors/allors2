namespace Allors.Meta
{
    public partial class RequestInterface
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}