namespace Tests.ProductTypeTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductTypeListTest : Test
    {
        public ProductTypeListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductTypes();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Types", this.Driver.Title);
        }
    }
}
