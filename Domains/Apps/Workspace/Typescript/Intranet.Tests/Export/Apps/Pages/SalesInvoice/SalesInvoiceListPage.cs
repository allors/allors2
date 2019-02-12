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

        public Anchor<SalesInvoiceListPage> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
