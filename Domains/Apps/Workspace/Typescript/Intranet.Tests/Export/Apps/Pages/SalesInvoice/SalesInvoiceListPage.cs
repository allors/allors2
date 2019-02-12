namespace Pages.SalesInvoicesOverviewTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SalesInvoiceListPage : MainPage
    {
        public SalesInvoiceListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
