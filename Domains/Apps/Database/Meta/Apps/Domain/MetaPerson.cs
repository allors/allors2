namespace Allors.Meta
{
    public partial class MetaPerson
    {
        public Tree AngularDetail;

        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;

            this.FirstName.RelationType.Workspace = true;
            this.LastName.RelationType.Workspace = true;
            this.MiddleName.RelationType.Workspace = true;
            this.Gender.RelationType.Workspace = true;
            this.Salutation.RelationType.Workspace = true;

            var person = this;

            this.AngularDetail = new Tree(person)
                .Add(M.Party.CurrentOrganisationContactRelationships,
                    new Tree(M.OrganisationContactRelationship)
                        .Add(M.OrganisationContactRelationship.ContactKinds)
                        .Add(M.OrganisationContactRelationship.Contact))
                .Add(M.Party.InactiveOrganisationContactRelationships,
                    new Tree(M.OrganisationContactRelationship)
                        .Add(M.OrganisationContactRelationship.ContactKinds)
                        .Add(M.OrganisationContactRelationship.Contact))
                .Add(M.Party.CurrentPartyContactMechanisms,
                    new Tree(M.PartyContactMechanism)
                        .Add(M.PartyContactMechanism.ContactPurposes)
                        .Add(M.PartyContactMechanism.ContactMechanism))
                .Add(M.Party.InactivePartyContactMechanisms,
                    new Tree(M.PartyContactMechanism)
                        .Add(M.PartyContactMechanism.ContactPurposes)
                        .Add(M.PartyContactMechanism.ContactMechanism))
                .Add(M.Party.GeneralCorrespondence,
                    new Tree(M.PostalAddress))
                .Add(M.Party.Locale,
                    new Tree(M.Locale)
                        .Add(M.Locale.Country)
                        .Add(M.Locale.Language));
        }
    }
}