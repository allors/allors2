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
            this.PostalBoundary.RelationType.Workspace = true;

            this.AngularDetail = new Tree(this)
                    .Add(M.PostalAddress.PostalBoundary, new Tree(M.PostalBoundary)
                        .Add(M.PostalBoundary.PostalCode)
                        .Add(M.PostalBoundary.Locality)
                        .Add(M.PostalBoundary.Country));
        }
    }
}