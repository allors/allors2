namespace Tests.Intranet.PartyRelationshipTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Components;
    using Tests.Intranet.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class SupplierRelationshipEditTest : Test
    {
        private readonly OrganisationListPage organisations;

        private readonly PartyRelationship editPartyRelationship;

        public SupplierRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var supplier = new OrganisationBuilder(this.Session).WithName("supplier").Build();


            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in allors.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            this.editPartyRelationship = new SupplierRelationshipBuilder(this.Session)
                .WithSupplier(supplier)
                .WithInternalOrganisation(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Create()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var internalOrganisation = extent.First(v => v.PartyName.Equals("Allors BVBA"));

            var organisationOverviewPage = this.organisations.Select(internalOrganisation);
            var page = organisationOverviewPage.NewCustomerRelationship();

            page.FromDate.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ThroughDate.Value = DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PartyRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
        }

        [Fact]
        public void Edit()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var extent = new Organisations(this.Session).Extent();
            var internalOrganisation = extent.First(v => v.PartyName.Equals("Allors BVBA"));

            var organisationOverviewPage = this.organisations.Select(internalOrganisation);
            var page = organisationOverviewPage.SelectPartyRelationship(this.editPartyRelationship);

            page.FromDate.Value = DateTimeFactory.CreateDate(2018, 12, 22);
            page.ThroughDate.Value = DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1);

            page.Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PartyRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
        }
    }
}
