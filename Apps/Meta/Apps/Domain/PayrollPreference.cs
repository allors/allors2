namespace Allors.Meta
{
    public partial class PayrollPreferenceClass
	{
	    internal override void AppsExtend()
        {
            this.PaymentMethod.RoleType.IsRequired = true;
            this.TimeFrequency.RoleType.IsRequired = true;
        }
	}
}