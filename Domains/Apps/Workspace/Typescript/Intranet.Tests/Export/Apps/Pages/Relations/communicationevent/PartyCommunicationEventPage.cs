namespace Intranet.Pages.Relations
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PartyCommunicationEventPage : MainPage
    {
        public PartyCommunicationEventPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button Edit => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));
    }
}
