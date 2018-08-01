namespace Intranet.Tests.AccountsReceivableInvoice
{
    using Xunit;

    [Collection("Test collection")]
    public class InvoicesOverviewTest : Test
    {
        public InvoicesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToAccountsReceivableInvoices();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Sales Invoices", this.Driver.Title);
        }
    }
}
