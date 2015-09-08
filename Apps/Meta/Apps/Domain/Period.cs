namespace Allors.Meta
{
    public partial class PeriodInterface
	{
	    internal override void AppsExtend()
        {
            this.FromDate.RoleType.IsRequired = true;
        }
	}
}