namespace Allors.Meta
{
    public partial class OwnBankAccountClass
    {
        internal override void AppsExtend()
        {
            this.BankAccount.RoleType.IsRequired = true;
        }
    }
}