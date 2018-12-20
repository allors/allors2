namespace Tests.Intranet.ProductTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductListTest : Test
    {
        public ProductListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Products", this.Driver.Title);
        }
    }
}
