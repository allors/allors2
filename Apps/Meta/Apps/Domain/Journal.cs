namespace Allors.Meta
{
    public partial class JournalClass
	{
	    internal override void AppsExtend()
        {
            this.Description.RoleType.IsRequired = true;
            this.InternalOrganisation.RoleType.IsRequired = true;
            this.JournalType.RoleType.IsRequired = true;
            this.ContraAccount.RoleType.IsRequired = true;
            this.BlockUnpaidTransactions.RoleType.IsRequired = true;
            this.CloseWhenInBalance.RoleType.IsRequired = true;
            this.UseAsDefault.RoleType.IsRequired = true;
        }
	}
}