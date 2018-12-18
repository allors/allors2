namespace Tests.Intranet.ProductQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductQuoteListTest : Test
    {
        public ProductQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToProductQuoteList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Quotes", this.Driver.Title);
        }
    }
}
