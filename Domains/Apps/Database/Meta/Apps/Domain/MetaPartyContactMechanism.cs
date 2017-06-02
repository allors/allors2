namespace Allors.Meta
{
    public partial class MetaPartyContactMechanism
    {
        public Tree AngularDetail;

        internal override void AppsExtend()
        {
            this.ContactMechanism.RelationType.Workspace = true;
            this.ContactPurposes.RelationType.Workspace = true;
            this.NonSolicitationIndicator.RelationType.Workspace = true;
            this.UseAsDefault.RelationType.Workspace = true;

            this.AngularDetail = new Tree(M.PartyContactMechanism)
                .Add(M.PartyContactMechanism.ContactPurposes)
                .Add(M.PartyContactMechanism.ContactMechanism);
        }
    }
}