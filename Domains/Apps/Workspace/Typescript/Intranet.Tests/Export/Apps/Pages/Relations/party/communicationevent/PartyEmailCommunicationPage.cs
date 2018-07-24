namespace Intranet.Pages.Relations
{
    using Allors.Meta;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PartyEmailCommunicationPage : MainPage
    {
        public PartyEmailCommunicationPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialDatetimePicker ScheduledStart => new MaterialDatetimePicker(this.Driver, M.EmailCommunication.ScheduledStart);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
