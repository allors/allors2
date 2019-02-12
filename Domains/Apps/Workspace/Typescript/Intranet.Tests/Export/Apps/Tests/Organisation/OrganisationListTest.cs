namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;

    using Pages.OrganisationTests;

    using Xunit;

    [Collection("Test collection")]
    public class OrganisationListTest : Test
    {
        private OrganisationListPage page;

        public OrganisationListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToOrganisationList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Organisations", this.Driver.Title);
        }

        [Fact]
        public void Table()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme0");
            var row = this.page.Table.FindRow(organisation);
            var cell = row.FindCell("name");

            Assert.Equal("Acme0", cell.Element.Text);
        }
    }
}
