namespace Allors.Meta
{
    public partial class PassportClass
    {
        internal override void AppsExtend()
        {
            this.Number.RoleType.IsRequired = true;
            this.Number.RoleType.IsUnique = true;
        }
    }
}