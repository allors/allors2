namespace Tests.Intranet.Relations
{
    using Allors.Meta;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

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
