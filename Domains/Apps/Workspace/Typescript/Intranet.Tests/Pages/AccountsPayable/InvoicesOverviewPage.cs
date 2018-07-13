namespace Intranet.Pages.AccountsPayable
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class InvoicesOverviewPage : MainPage
    {
        public InvoicesOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
