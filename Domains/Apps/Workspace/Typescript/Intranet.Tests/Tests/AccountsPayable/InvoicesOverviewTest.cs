namespace Intranet.Tests.AccountsPayable
{
    using Xunit;

    [Collection("Test collection")]
    public class InvoicesOverviewTest : Test
    {
        public InvoicesOverviewTest(TestFixture fixture)
            : base(fixture)
        {
            var dashboard = this.Login();
            dashboard.Sidenav.NavigateToAccountsPayableInvoices();
        }

        [Fact]
        public void Title()
        {
            Assert.Equal("Purchase Invoices", this.Driver.Title);
        }
    }
}
