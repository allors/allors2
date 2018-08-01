namespace Intranet.Tests.CataloguesCategory
{
    using Xunit;

    [Collection("Test collection")]
    public class CategoriesOverviewTest : Test
    {
        public CategoriesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToCategories();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Categories", this.Driver.Title);
        }
    }
}
