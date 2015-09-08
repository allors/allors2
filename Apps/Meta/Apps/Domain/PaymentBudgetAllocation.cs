namespace Allors.Meta
{
    public partial class PaymentBudgetAllocationClass
	{
	    internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;
            this.BudgetItem.RoleType.IsRequired = true;
            this.Payment.RoleType.IsRequired = true;
        }
	}
}