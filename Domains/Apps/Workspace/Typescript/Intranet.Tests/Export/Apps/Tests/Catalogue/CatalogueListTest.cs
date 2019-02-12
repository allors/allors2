namespace Tests.CatalogueTests
{
    using Xunit;

    [Collection("Test collection")]
    public class CatalogueListTest : Test
    {
        public CatalogueListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToCatalogueList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Catalogues", this.Driver.Title);
        }
    }
}
