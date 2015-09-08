namespace Allors.Meta
{
    public partial class GlBudgetAllocationClass
	{
	    internal override void AppsExtend()
        {
            this.AllocationPercentage.RoleType.IsRequired = true;
            this.BudgetItem.RoleType.IsRequired = true;
            this.GeneralLedgerAccount.RoleType.IsRequired = true;
        }
	}
}