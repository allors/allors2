namespace Tests.ProductQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class ProductQuoteListTest : Test
    {
        public ProductQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToProductQuotes();
        }

        [Fact]
        public void Title() => Assert.Equal("Quotes", this.Driver.Title);
    }
}
