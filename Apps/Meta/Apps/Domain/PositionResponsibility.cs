namespace Allors.Meta
{
    public partial class PositionResponsibilityClass
	{
	    internal override void AppsExtend()
        {
            this.Position.RoleType.IsRequired = true;
            this.Responsibility.RoleType.IsRequired = true;
        }
	}
}