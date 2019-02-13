namespace Tests.PartyRelationshipTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.PersonTests;

    using Xunit;

    [Collection("Test collection")]
    public class PersonEmploymentEditTest : Test
    {
        private readonly PersonListPage people;

        private readonly Employment editPartyRelationship;

        private readonly Person employee;

        public PersonEmploymentEditTest(TestFixture fixture)
            : base(fixture)
        {
            var allors = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            this.employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in allors.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            this.editPartyRelationship = new EmploymentBuilder(this.Session)
                .WithEmployee(this.employee)
                .WithEmployer(allors)
                .Build();

            this.Session.Derive();
            this.Session.Commit();

            var dashboard = this.Login();
            this.people = dashboard.Sidenav.NavigateToPersonList();
        }

        [Fact]
        public void Create()
        {
            var before = new Employments(this.Session).Extent().ToArray();

            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var personOverviewPage = this.people.Select(this.employee);
            var page = personOverviewPage.NewEmployment();

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, partyRelationship.Employer);
            Assert.Equal(this.employee, partyRelationship.Employee);
        }

        [Fact]
        public void Edit()
        {
            var before = new Employments(this.Session).Extent().ToArray();

            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");

            var personOverviewPage = this.people.Select(this.employee);
            var page = personOverviewPage.SelectPartyRelationship(this.editPartyRelationship);

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, this.editPartyRelationship.Employer);
            Assert.Equal(this.employee, this.editPartyRelationship.Employee);
        }
    }
}
