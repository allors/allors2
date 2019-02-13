namespace Tests.PartyRelationshipTests
{
    using System.Linq;

    using Allors;
    using Allors.Domain;
    using Allors.Meta;

    using Angular;

    using Pages.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class InternalOrganisationEmploymentEditTest : Test
    {
        private readonly OrganisationListPage organisationListPage;

        public InternalOrganisationEmploymentEditTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.organisationListPage = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Create()
        {
            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship relationship in employer.PartyRelationshipsWhereParty)
            {
                relationship.Delete();
            }

            this.Session.Derive();
            this.Session.Commit();

            var before = new Employments(this.Session).Extent().ToArray();

            var page = this.organisationListPage.Select(employer).NewEmployment();

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Employee.Set(employee.PartyName)
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length + 1);

            var partyRelationship = after.Except(before).First();

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, partyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, partyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, partyRelationship.Employer);
            Assert.Equal(employee, partyRelationship.Employee);
        }

        [Fact]
        public void Edit()
        {
            var employer = new Organisations(this.Session).FindBy(M.Organisation.Name, "Allors BVBA");
            var employee = new PersonBuilder(this.Session).WithLastName("employee").Build();

            // Delete all existing for the new one to be in the first page of the list.
            foreach (PartyRelationship partyRelationship in employer.PartyRelationshipsWhereParty)
            {
                partyRelationship.Delete();
            }

            var editPartyRelationship = new EmploymentBuilder(this.Session)
                .WithEmployee(employee)
                .WithEmployer(employer)
                .Build();

            this.Session.Derive();
            this.Session.Commit();
            
            var before = new Employments(this.Session).Extent().ToArray();

            var page = this.organisationListPage.Select(employer).SelectPartyRelationship(editPartyRelationship);

            page.FromDate.Set(DateTimeFactory.CreateDate(2018, 12, 22))
                .ThroughDate.Set(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1))
                .Save.Click();

            this.Driver.WaitForAngular();
            this.Session.Rollback();

            var after = new Employments(this.Session).Extent().ToArray();

            Assert.Equal(after.Length, before.Length);

            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).Date, this.editPartyRelationship.FromDate.Date.ToUniversalTime().Date);
            //Assert.Equal(DateTimeFactory.CreateDate(2018, 12, 22).AddYears(1).Date, this.editPartyRelationship.ThroughDate.Value.Date.ToUniversalTime().Date);
            Assert.Equal(employer, editPartyRelationship.Employer);
            Assert.Equal(employee, editPartyRelationship.Employee);
        }
    }
}
