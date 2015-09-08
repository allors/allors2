namespace Allors.Meta
{
    public partial class OrganisationGlAccountBalanceClass
	{
	    internal override void AppsExtend()
        {
            this.Amount.RoleType.IsRequired = true;
            this.OrganisationGlAccount.RoleType.IsRequired = true;
        }
	}
}