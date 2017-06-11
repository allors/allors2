namespace Allors.Meta
{
    public partial class MetaOrganisation
    {
        public Tree AngularList;
        public Tree AngularDetail;

        internal override void AppsExtend()
        {
            this.Delete.Workspace = true;

            this.Name.RelationType.Workspace = true;
            this.LogoImage.RelationType.Workspace = true;
            this.LegalForm.RelationType.Workspace = true;

            var organisation = this;

            this.AngularList = new Tree(organisation)
                .Add(M.Party.GeneralCorrespondence,
                    new Tree(M.PostalAddress));

            this.AngularDetail = new Tree(organisation)
                .Add(organisation.IndustryClassification)
                .Add(M.Party.CurrentContacts,
                    new Tree(M.Person)
                        .Add(M.Person.PartyContactMechanisms, new Tree(M.PartyContactMechanism)
                            .Add(M.PartyContactMechanism.ContactPurposes)
                            .Add(M.PartyContactMechanism.ContactMechanism)))
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
                        .Add(M.Locale.Language))
                .Add(organisation.LogoImage,
                    new Tree(M.Media)
                        .Add(M.Media.MediaContent,
                            new Tree(M.MediaContent)
                                .Add(M.MediaContent.Type)
                                .Add(M.MediaContent.Data)));
        }
    }
}