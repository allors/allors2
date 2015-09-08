namespace Allors.Meta
{
    public partial class PaymentInterface
	{
	    internal override void AppsExtend()
        {
            this.EffectiveDate.RoleType.IsRequired = true;
            this.Amount.RoleType.IsRequired = true;
        }
	}
}