namespace Tests.Intranet.Products
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductTypesOverviewTest : Test
    {
        public ProductTypesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductTypes();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Types", this.Driver.Title);
        }
    }
}
