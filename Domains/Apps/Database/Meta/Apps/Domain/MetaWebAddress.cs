namespace Allors.Meta
{
    public partial class MetaWebAddress
    {
        internal override void AppsExtend()
        {
            this.ElectronicAddressString.RelationType.Workspace = true;
        }
    }
}