using src.allors.material.apps.objects.organisation.list;

namespace Tests.OrganisationTests
{
    using Allors.Domain;
    using Allors.Meta;
    using Xunit;

    [Collection("Test collection")]
    public class OrganisationListTest : Test
    {
        private OrganisationListComponent page;

        public OrganisationListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToOrganisations();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Organisations", this.Driver.Title);
        }

        [Fact]
        public void Table()
        {
            var organisation = new Organisations(this.Session).FindBy(M.Organisation.Name, "Acme");
            var row = this.page.Table.FindRow(organisation);
            var cell = row.FindCell("name");

            Assert.Equal("Acme", cell.Element.Text);
        }
    }
}
