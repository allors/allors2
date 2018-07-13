namespace Intranet.Tests.Orders
{
    using Intranet.Pages.Orders;

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

        [Fact]
        public void Search()
        {
            var page = new QuotesOverviewPage(this.Driver);

            page.Company.Text = "acme";
        }
    }
}
