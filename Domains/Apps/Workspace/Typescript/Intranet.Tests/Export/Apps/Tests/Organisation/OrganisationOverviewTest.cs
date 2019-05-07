using src.allors.material.apps.objects.organisation.list;

namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationOverviewTest : Test
    {
        private readonly OrganisationListComponent organisationListPage;

        public OrganisationOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.organisationListPage = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");

            this.organisationListPage.Select(organisation);

            Assert.Equal("Organisation", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");
            var page = this.organisationListPage.Select(organisation);

            page.List.Click();

            Assert.Equal("Organisations", this.Driver.Title);
        }
    }
}
