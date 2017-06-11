namespace Allors.Meta
{
    public partial class MetaPostalAddress
    {
        public Tree AngularDetail;

        internal override void AppsExtend()
        {
            this.Address1.RelationType.Workspace = true;
            this.Address2.RelationType.Workspace = true;
            this.Address3.RelationType.Workspace = true;
            this.Description.RelationType.Workspace = true;
        }
    }
}