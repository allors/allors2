namespace Tests.ProductCategoryTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductCategoryListTest : Test
    {
        public ProductCategoryListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductCategories();
        }

        [Fact]
        public void Title() => Assert.Equal("Categories", this.Driver.Title);
    }
}
