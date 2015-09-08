namespace Allors.Meta
{
    public partial class ProductConfigurationClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}