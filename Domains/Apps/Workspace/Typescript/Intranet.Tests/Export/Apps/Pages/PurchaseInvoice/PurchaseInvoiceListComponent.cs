using src.app.main;

namespace Pages.PurchaseInvoiceTest
{
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PurchaseInvoiceListComponent : MainComponent
    {
        public PurchaseInvoiceListComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor<PurchaseInvoiceListComponent> AddNew => this.Anchor(By.LinkText("Add New"));
    }
}
