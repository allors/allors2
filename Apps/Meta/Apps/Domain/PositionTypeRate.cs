namespace Allors.Meta
{
    public partial class PositionTypeRateClass
	{
	    internal override void AppsExtend()
        {
            this.Rate.RoleType.IsRequired = true;
            this.RateType.RoleType.IsRequired = true;
            this.TimeFrequency.RoleType.IsRequired = true;
        }
	}
}