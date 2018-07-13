namespace Intranet.Tests.Relations
{
    using Intranet.Pages.Relations;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationsOverviewTest : Test
    {
        public OrganisationsOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Organisations", this.Driver.Title);
        }

        [Fact]
        public void Search()
        {
            var page = new OrganisationsOverviewPage(this.Driver);

            page.Name.Text = "acme";
        }
    }
}
