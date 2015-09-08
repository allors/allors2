namespace Allors.Meta
{
    public partial class PaymentMethodInterface
    {
        internal override void AppsExtend()
        {
            this.CurrentBalance.RoleType.IsRequired = true;
            this.IsActive.RoleType.IsRequired = true;
        }
    }
}