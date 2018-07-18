namespace Intranet.Pages.Orders
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class QuotesOverviewPage : MainPage
    {
        public QuotesOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Company => new Input(this.Driver, formControlName: "company");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
