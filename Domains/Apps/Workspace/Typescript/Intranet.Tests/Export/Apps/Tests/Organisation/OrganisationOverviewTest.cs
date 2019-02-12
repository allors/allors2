namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;

    using Pages.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationOverviewTest : Test
    {
        private readonly OrganisationListPage organisations;

        public OrganisationOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Title()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");
            this.organisations.Select(organisation);
            Assert.Equal("Organisation", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");
            var overviewPage = this.organisations.Select(organisation);
            Assert.Equal("Organisation", this.Driver.Title);

            overviewPage.List.Click();

            Assert.Equal("Organisations", this.Driver.Title);
        }
    }
}
