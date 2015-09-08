namespace Allors.Meta
{
    public partial class PositionClass
	{
	    internal override void AppsExtend()
        {
            this.PositionType.RoleType.IsRequired = true;
            this.Organisation.RoleType.IsRequired = true;
            this.ActualFromDate.RoleType.IsRequired = true;
        }
	}
}