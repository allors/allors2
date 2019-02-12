namespace Tests.ProductTypeTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductTypeListTest : Test
    {
        public ProductTypeListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductTypeList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Product Types", this.Driver.Title);
        }
    }
}
