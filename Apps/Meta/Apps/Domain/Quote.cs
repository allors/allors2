namespace Allors.Meta
{
    public partial class QuoteInterface
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}