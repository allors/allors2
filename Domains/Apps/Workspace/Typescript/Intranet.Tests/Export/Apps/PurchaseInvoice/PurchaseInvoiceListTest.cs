namespace Tests.Intranet.PurchaseInvoiceTest
{
    using Xunit;

    [Collection("Test collection")]
    public class PurchaseInvoiceListTest : Test
    {
        public PurchaseInvoiceListTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToPurchaseInvoiceList();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Purchase Invoices", this.Driver.Title);
        }
    }
}
