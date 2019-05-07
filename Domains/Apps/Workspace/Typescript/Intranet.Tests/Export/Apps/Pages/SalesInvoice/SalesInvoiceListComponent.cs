using src.app.main;

namespace Pages.SalesInvoicesOverviewTest
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SalesInvoiceListComponent : MainComponent
    {
        public SalesInvoiceListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor<SalesInvoiceListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
