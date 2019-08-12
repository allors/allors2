
namespace Tests.CatalogueTests
{
    using src.allors.material.@base.objects.catalogue.list;
    using Xunit;

    [Collection("Test collection")]
    public class CatalogueListTest : Test
    {
        private readonly CataloguesListComponent page;

        public CatalogueListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.page = this.Sidenav.NavigateToCatalogues();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Catalogues", this.Driver.Title);
        }
    }
}
