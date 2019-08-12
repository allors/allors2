using src.allors.material.apps.objects.organisation.list;
using src.allors.material.apps.objects.organisation.overview;

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
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");

            this.organisationListPage.Table.DefaultAction(organisation);
            new OrganisationOverviewComponent(this.organisationListPage.Driver);

            Assert.Equal("Organisation", this.Driver.Title);
        }

        [Fact]
        public void NavigateToList()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");
            this.organisationListPage.Table.DefaultAction(organisation);
            var organisationOverview = new OrganisationOverviewComponent(this.organisationListPage.Driver);

            organisationOverview.Organisations.Click();

            Assert.Equal("Organisations", this.Driver.Title);
        }
    }
}
