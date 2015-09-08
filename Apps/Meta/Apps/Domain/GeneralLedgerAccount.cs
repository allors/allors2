namespace Allors.Meta
{
    public partial class GeneralLedgerAccountClass
	{
	    internal override void AppsExtend()
        {
            this.CashAccount.RoleType.IsRequired = true;
            this.CostCenterAccount.RoleType.IsRequired = true;
            this.CostCenterRequired.RoleType.IsRequired = true;
            this.CostUnitAccount.RoleType.IsRequired = true;
            this.CostUnitRequired.RoleType.IsRequired = true;
            this.ReconciliationAccount.RoleType.IsRequired = true;
            this.Protected.RoleType.IsRequired = true;
            this.AccountNumber.RoleType.IsRequired = true;
            this.Name.RoleType.IsRequired = true;
            this.BalanceSheetAccount.RoleType.IsRequired = true;
            this.Side.RoleType.IsRequired = true;
            this.GeneralLedgerAccountType.RoleType.IsRequired = true;
            this.GeneralLedgerAccountGroup.RoleType.IsRequired = true;
        }
	}
}