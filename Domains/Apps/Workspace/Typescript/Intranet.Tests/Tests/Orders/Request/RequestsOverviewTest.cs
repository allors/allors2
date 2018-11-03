namespace Intranet.Tests.OrdersRequest
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
    }
}
