namespace Tests.RequestsForQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class RequestsForQuoteListTest : Test
    {
        public RequestsForQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToRequestsForQuote();
        }

        [Fact]
        public void Title() => Assert.Equal("Requests", this.Driver.Title);
    }
}
