namespace Tests.Intranet.SalesInvoicesOverviewTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class SalesInvoiceListPage : MainPage
    {
        public SalesInvoiceListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
