namespace Allors.Meta
{
    public partial class PayHistoryClass
	{
	    internal override void AppsExtend()
        {
            this.Employment.RoleType.IsRequired = true;
            this.TimeFrequency.RoleType.IsRequired = true;
        }
	}
}