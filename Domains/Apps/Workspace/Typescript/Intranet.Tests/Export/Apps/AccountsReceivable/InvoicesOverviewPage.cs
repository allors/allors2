namespace Tests.Intranet.AccountsReceivable
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class InvoicesOverviewPage : MainPage
    {
        public InvoicesOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
