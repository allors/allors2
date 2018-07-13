namespace Intranet.Tests.Orders
{
    using Intranet.Pages.Orders;

    using Xunit;

    [Collection("Test collection")]
    public class RequestsOverviewTest : Test
    {
        public RequestsOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToRequests();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Requests", this.Driver.Title);
        }

        [Fact]
        public void Search()
        {
            var page = new RequestsOverviewPage(this.Driver);

            page.Company.Text = "acme";
        }
    }
}
