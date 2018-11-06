namespace Tests.Intranet.OrdersSalesOrder
{
    using Tests.Intranet.Orders;

    using Xunit;

    [Collection("Test collection")]
    public class OrdersOverviewTest : Test
    {
        public OrdersOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToOrders();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Sales Orders", this.Driver.Title);
        }
    }
}
