namespace Intranet.Tests.CataloguesCatalogue
{
    using Xunit;

    [Collection("Test collection")]
    public class CataloguesOverviewTest : Test
    {
        public CataloguesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToCatalogues();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Catalogues", this.Driver.Title);
        }
    }
}
