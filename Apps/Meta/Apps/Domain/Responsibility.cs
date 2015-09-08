namespace Allors.Meta
{
    public partial class ResponsibilityClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}