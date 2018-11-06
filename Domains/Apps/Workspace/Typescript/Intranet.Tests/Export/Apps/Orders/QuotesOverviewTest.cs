namespace Tests.Intranet.OrdersQuote
{
    using Tests.Intranet.Orders;

    using Xunit;

    [Collection("Test collection")]
    public class QuotesOverviewTest : Test
    {
        public QuotesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToQuotes();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Quotes", this.Driver.Title);
        }
    }
}
