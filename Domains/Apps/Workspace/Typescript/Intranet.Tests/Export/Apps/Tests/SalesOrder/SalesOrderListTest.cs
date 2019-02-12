namespace Tests.SalesOrderTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SalesOrderListTest : Test
    {
        public SalesOrderListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToSalesOrderList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Sales Orders", this.Driver.Title);
        }
    }
}
