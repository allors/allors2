namespace Tests.Intranet.Products
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductCharacteristicsOverviewTest : Test
    {
        public ProductCharacteristicsOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductCharacteristics();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Characteristics", this.Driver.Title);
        }
    }
}
