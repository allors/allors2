namespace Allors.Meta
{
    public partial class OrganisationGlAccountClass
	{
	    internal override void AppsExtend()
        {
			this.HasBankStatementTransactions.RoleType.IsRequired = true;

            this.GeneralLedgerAccount.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
		}
	}
}