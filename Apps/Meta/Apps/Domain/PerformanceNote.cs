namespace Allors.Meta
{
    public partial class PerformanceNoteClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.Employee.RoleType.IsRequired = true;
        }
	}
}