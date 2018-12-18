namespace Tests.Intranet.ProductCharacteristicTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductCharacteristicListTest : Test
    {
        public ProductCharacteristicListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductCharacteristicList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Characteristics", this.Driver.Title);
        }
    }
}
