namespace Allors.Meta
{
    public partial class MetaCustomerRelationship
    {

        internal override void AppsExtend()
        {
            this.FromDate.RelationType.Workspace = true;
            this.ThroughDate.RelationType.Workspace = true;
            this.InternalOrganisation.RelationType.Workspace = true;
            this.Customer.RelationType.Workspace = true;
        }
    }
}