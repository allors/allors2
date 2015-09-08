namespace Allors.Meta
{
    public partial class PositionTypeClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
        }
	}
}