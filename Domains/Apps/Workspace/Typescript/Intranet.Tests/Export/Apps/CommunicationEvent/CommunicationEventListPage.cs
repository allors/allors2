namespace Tests.Intranet.CommunicationEventTests
{
    using Allors.Domain;
    using Allors.Meta;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

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
