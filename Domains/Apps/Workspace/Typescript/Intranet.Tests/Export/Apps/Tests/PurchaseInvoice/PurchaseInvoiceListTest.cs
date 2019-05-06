namespace Tests.PurchaseInvoiceTest
{
    using Xunit;

    [Collection("Test collection")]
    public class PurchaseInvoiceListTest : Test
    {
        public PurchaseInvoiceListTest(TestFixture fixture)
            : base(fixture)
        {
            this.Login();
            this.Sidenav.NavigateToPurchaseInvoices();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Purchase Invoices", this.Driver.Title);
        }
    }
}
