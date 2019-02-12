namespace Tests.CatalogueTests
{
    using Pages.CatalogueTests;

    using Xunit;

    [Collection("Test collection")]
    public class CatalogueListTest : Test
    {
        private readonly CatalogueListPage page;

        public CatalogueListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            this.page = dashboard.Sidenav.NavigateToCatalogueList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Catalogues", this.Driver.Title);
        }
    }
}
