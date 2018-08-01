namespace Intranet.Pages.Relations
{
    using Allors.Meta;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class CommunicationEventWorkTaskPage : MainPage
    {
        public CommunicationEventWorkTaskPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput Name => new MaterialInput(this.Driver, M.WorkTask.Name);

        public MaterialDatetimePicker ScheduledStart => new MaterialDatetimePicker(this.Driver, M.WorkTask.ScheduledStart);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
