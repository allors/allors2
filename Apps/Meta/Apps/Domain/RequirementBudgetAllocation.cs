namespace Allors.Meta
{
    public partial class RequirementBudgetAllocationClass
	{
	    internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;
            this.BudgetItem.RoleType.IsRequired = true;
            this.Requirement.RoleType.IsRequired = true;
        }
	}
}