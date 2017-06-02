namespace Allors.Meta
{
    public partial class MetaCountry
    {
        internal override void AppsExtend()
        {
            this.Name.RelationType.Workspace = true;
            this.Abbreviation.RelationType.Workspace = true;
            this.IsoCode.RelationType.Workspace = true;
        }
    }
}