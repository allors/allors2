namespace Allors.Meta
{
    public partial class ProductCategoryClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}