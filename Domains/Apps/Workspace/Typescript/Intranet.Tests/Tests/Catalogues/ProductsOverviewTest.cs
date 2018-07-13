namespace Intranet.Tests.Catalogues
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductsOverviewTest : Test
    {
        public ProductsOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProducts();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Products", this.Driver.Title);
        }
    }
}
