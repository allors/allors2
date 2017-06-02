namespace Allors.Meta
{
    public partial class MetaEmailAddress
    {
        internal override void AppsExtend()
        {
            this.ElectronicAddressString.RelationType.Workspace = true;
        }
    }
}