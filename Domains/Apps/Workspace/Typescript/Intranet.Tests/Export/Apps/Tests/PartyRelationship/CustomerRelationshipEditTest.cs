namespace Tests.PartyRelationshipTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Tests.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class CustomerRelationshipEditTest : Test
    {
        private readonly PersonListPage people;

        private readonly PartyRelationship editPartyRelationship;

        public CustomerRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            this.editPartyRelationship = new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(person)
                .WithInternalOrganisation(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var personOverview = this.people.Select(person);
            var page = personOverview.NewCustomerRelationship();

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
            var extent = new People(this.Session).Extent();
            var person = extent.First(v => v.PartyName.Equals("John0 Doe0"));

            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var personOverview = this.people.Select(person);

            var page = personOverview.SelectPartyRelationship(this.editPartyRelationship);

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
