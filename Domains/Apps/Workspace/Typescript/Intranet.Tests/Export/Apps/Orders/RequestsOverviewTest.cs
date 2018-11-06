namespace Tests.Intranet.OrdersRequest
{
    using Tests.Intranet.Orders;

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
