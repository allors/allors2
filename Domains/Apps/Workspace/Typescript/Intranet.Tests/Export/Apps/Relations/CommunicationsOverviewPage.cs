namespace Tests.Intranet.Relations
{
    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

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
