namespace Allors.Meta
{
    public partial class MetaTelecommunicationsNumber
    {
        internal override void AppsExtend()
        {
            this.CountryCode.RelationType.Workspace = true;
            this.AreaCode.RelationType.Workspace = true;
            this.ContactNumber.RelationType.Workspace = true;
        }
    }
}