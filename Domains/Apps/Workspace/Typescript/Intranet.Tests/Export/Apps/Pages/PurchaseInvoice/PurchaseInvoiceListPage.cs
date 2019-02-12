namespace Pages.PurchaseInvoiceTest
{
    using Angular.Html;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PurchaseInvoiceListPage : MainPage
    {
        public PurchaseInvoiceListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
