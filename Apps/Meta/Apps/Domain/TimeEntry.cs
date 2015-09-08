namespace Allors.Meta
{
    public partial class TimeEntryClass
	{
	    internal override void AppsExtend()
        {
			this.GrossMargin.RoleType.IsRequired = true;

            this.UnitOfMeasure.RoleType.IsRequired = true;
            this.Cost.RoleType.IsRequired = true;
		}
	}
}