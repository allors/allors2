namespace Tests.Intranet.Relations
{
    using System;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class PartyCommunicationEventPage : MainPage
    {
        public PartyCommunicationEventPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button Edit => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public Button AddNew => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Add new')]"));
    }
}
