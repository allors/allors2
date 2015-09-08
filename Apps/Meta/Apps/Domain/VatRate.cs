namespace Allors.Meta
{
    public partial class VatRateClass
	{
	    internal override void AppsExtend()
        {
            this.Rate.RoleType.IsRequired = true;
        }
	}
}