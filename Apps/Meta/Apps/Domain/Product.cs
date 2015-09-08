namespace Allors.Meta
{
    public partial class ProductInterface
    {
        internal override void AppsExtend()
        {
            this.Name.RoleType.IsRequired = true;
            this.VatRate.RoleType.IsRequired = true;
            this.SoldBy.RoleType.IsRequired = true;
        }
    }
}