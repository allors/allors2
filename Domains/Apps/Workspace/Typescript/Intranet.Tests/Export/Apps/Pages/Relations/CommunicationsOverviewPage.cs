namespace Intranet.Pages.Relations
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class CommunicationsOverviewPage : MainPage
    {
        public CommunicationsOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Subject => new Input(this.Driver, formControlName: "subject");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
