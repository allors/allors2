namespace Allors.Meta
{
    public partial class OwnCreditCardClass
	{
	    internal override void AppsExtend()
        {
            this.CreditCard.RoleType.IsRequired = true;
        }
	}
}