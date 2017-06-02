namespace Allors.Meta
{
    public partial class MetaPostalBoundary
    {

        internal override void AppsExtend()
        {
            this.PostalCode.RelationType.Workspace = true;
            this.Locality.RelationType.Workspace = true;
            this.Country.RelationType.Workspace = true;
            this.Region.RelationType.Workspace = true;
        }
    }
}