namespace Pages.CommunicationEventTests
{
    using Allors.Domain;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class CommunicationEventListPage : MainPage
    {
        public CommunicationEventListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public CommunicationEventListPage Select(CommunicationEvent communicationEvent)
        {
            var row = this.Table.FindRow(communicationEvent);
            var cell = row.FindCell("subject");
            cell.Click();

            return new CommunicationEventListPage(this.Driver);
        }
    }
}
