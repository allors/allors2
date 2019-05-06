using src.allors.material.apps.objects.person.list;

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
        private readonly PersonListComponent personListPage;

        public CustomerRelationshipEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.personListPage = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var page = this.personListPage.Select(person).NewCustomerRelationship();

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Save.Click();

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
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var people = new People(this.Session).Extent();
            var person = people.First(v => v.PartyName.Equals("John0 Doe0"));

            var editPartyRelationship = new CustomerRelationshipBuilder(this.Session)
                .WithCustomer(person)
                .WithInternalOrganisation(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var before = new PartyRelationships(this.Session).Extent().ToArray();

            var personOverview = this.personListPage.Select(person);

            var page = personOverview.SelectPartyRelationship(editPartyRelationship);

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new PartyRelationships(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
        }
    }
}
