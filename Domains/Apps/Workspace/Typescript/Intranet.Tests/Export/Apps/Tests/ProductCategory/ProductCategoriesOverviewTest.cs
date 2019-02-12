namespace Tests.ProductCategoryTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductCategoryListTest : Test
    {
        public ProductCategoryListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToCategoryList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Categories", this.Driver.Title);
        }
    }
}
