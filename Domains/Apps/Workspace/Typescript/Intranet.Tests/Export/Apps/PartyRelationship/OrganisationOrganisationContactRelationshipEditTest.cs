namespace Tests.Intranet.PartyRelationshipTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;

    using Tests.Components;
    using Tests.Intranet.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationOrganisationContactRelationshipEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        private readonly OrganisationContactRelationship editPartyRelationship;

        private readonly Organisation organisation;

        private readonly Person contact;

        public OrganisationOrganisationContactRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            this.organisation = new OrganisationBuilder(this.Session).WithName("organisation").Build();
            this.contact = new PersonBuilder(this.Session).WithLastName("contact").Build();

            this.editPartyRelationship = new OrganisationContactRelationshipBuilder(this.Session)
                .WithContactKind(new OrganisationContactKinds(this.Session).GeneralContact)
                .WithContact(this.contact)
                .WithOrganisation(this.organisation)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Create()
        {
            var before = new OrganisationContactRelationships(this.Session).Extent().ToArray();

            var organisationOverviewPage = this.organisations.Select(this.organisation);
            var page = organisationOverviewPage.NewOrganisationContactRelationship();

            page.FromDate.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ThroughDate.Value = DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1);
            page.ContactKinds.Toggle(new OrganisationContactKinds(this.Session).SalesContact.Description);
            page.Contact.Value = this.contact.PartyName;

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrganisationContactRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(2, partyRelationship.ContactKinds.Count);
            Assert.Contains(new OrganisationContactKinds(this.Session).GeneralContact, partyRelationship.ContactKinds);
            Assert.Contains(new OrganisationContactKinds(this.Session).SalesContact, partyRelationship.ContactKinds);
            Assert.Equal(this.organisation, partyRelationship.Organisation);
            Assert.Equal(this.contact, partyRelationship.Contact);
        }

        [Fact]
        public void Edit()
        {
            var before = new OrganisationContactRelationships(this.Session).Extent().ToArray();

            var organisationOverviewPage = this.organisations.Select(this.organisation);
            var page = organisationOverviewPage.SelectPartyRelationship(this.editPartyRelationship);

            page.FromDate.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ThroughDate.Value = DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1);
            page.ContactKinds.Toggle(new OrganisationContactKinds(this.Session).GeneralContact.Description);
            page.ContactKinds.Toggle(new OrganisationContactKinds(this.Session).SalesContact.Description);
            page.ContactKinds.Toggle(new OrganisationContactKinds(this.Session).SupplierContact.Description);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new OrganisationContactRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(2, this.editPartyRelationship.ContactKinds.Count);
            Assert.Contains(new OrganisationContactKinds(this.Session).SalesContact, this.editPartyRelationship.ContactKinds);
            Assert.Contains(new OrganisationContactKinds(this.Session).SupplierContact, this.editPartyRelationship.ContactKinds);
            Assert.Equal(this.organisation, this.editPartyRelationship.Organisation);
            Assert.Equal(this.contact, this.editPartyRelationship.Contact);
        }
    }
}
