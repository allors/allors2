namespace Allors.Meta
{
    public partial class PositionReportingStructureClass
	{
	    internal override void AppsExtend()
        {
            this.Position.RoleType.IsRequired = true;
            this.ManagedByPosition.RoleType.IsRequired = true;
        }
	}
}