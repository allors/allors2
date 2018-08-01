namespace Intranet.Pages.Relations
{
    using System;
    using Intranet.Tests;

    using OpenQA.Selenium;

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
