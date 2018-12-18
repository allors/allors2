namespace Tests.Intranet.RequestsForQuoteTest
{
    using Xunit;

    [Collection("Test collection")]
    public class RequestsForQuoteListTest : Test
    {
        public RequestsForQuoteListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToRequestForQuoteList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Requests", this.Driver.Title);
        }
    }
}
