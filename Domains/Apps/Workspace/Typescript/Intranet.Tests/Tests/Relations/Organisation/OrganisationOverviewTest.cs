namespace Intranet.Tests.RelationsOrganisation
{
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationOverviewTest : Test
    {
        private readonly OrganisationsOverviewPage organisations;

        public OrganisationOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.organisations = dashboard.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Person.PartyName, "Acme");
            this.organisations.Select(organisation);
            Assert.Equal("Organisation Overview", this.Driver.Title);
        }
    }
}
