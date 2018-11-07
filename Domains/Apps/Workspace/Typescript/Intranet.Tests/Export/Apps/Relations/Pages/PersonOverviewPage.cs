namespace Tests.Intranet.Relations
{
    using Allors.Domain;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public MaterialList List => new MaterialList(this.Driver);

        public PersonEditPage Edit()
        {
            this.EditButton.Click();
            return new PersonEditPage(this.Driver);
        }

        internal PartyCommunicationEventPage Select(CommunicationEvent communicationEvent)
        {
            var listItem = this.List.FindListItem(communicationEvent);
            listItem.Click();
            return new PartyCommunicationEventPage(this.Driver);
        }
    }
}
