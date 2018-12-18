namespace Tests.Intranet.PurchaseInvoiceTest
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class PurchaseInvoiceListPage : MainPage
    {
        public PurchaseInvoiceListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
