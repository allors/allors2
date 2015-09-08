namespace Allors.Meta
{
    public partial class PaymentApplicationClass
	{
	    internal override void AppsExtend()
        {
            this.AmountApplied.RoleType.IsRequired = true;
        }
	}
}