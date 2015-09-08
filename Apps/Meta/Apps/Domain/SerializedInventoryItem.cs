namespace Allors.Meta
{
    public partial class SerializedInventoryItemClass
    {
        internal override void AppsExtend()
        {
            this.CurrentObjectState.RoleType.IsRequired = true;
            this.SerialNumber.RoleType.IsRequired = true;
            this.SerialNumber.RoleType.IsUnique = true;
        }
    }
}