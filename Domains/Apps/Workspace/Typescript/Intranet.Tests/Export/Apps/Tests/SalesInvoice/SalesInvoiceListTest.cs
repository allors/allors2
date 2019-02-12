namespace Tests.SalesInvoicesOverviewTest
{
    using Xunit;

    [Collection("Test collection")]
    public class SalesInvoiceListTest : Test
    {
        public SalesInvoiceListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToSalesInvoiceList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Sales Invoices", this.Driver.Title);
        }
    }
}
