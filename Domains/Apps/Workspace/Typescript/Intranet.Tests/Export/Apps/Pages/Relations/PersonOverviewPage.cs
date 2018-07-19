namespace Intranet.Pages.Relations
{
    using Allors.Domain;

    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList List => new MaterialList(this.Driver);

        public PersonPage Edit()
        {
            this.EditButton.Click();
            return new PersonPage(this.Driver);
        }

        internal PartyCommunicationEventPage Select(CommunicationEvent communicationEvent)
        {
            var listItem = this.List.FindListItem(communicationEvent);
            listItem.Click();
            return new PartyCommunicationEventPage(this.Driver);
        }
    }
}
