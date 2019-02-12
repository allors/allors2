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

        public Anchor<CommunicationEventListPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public MaterialTable<CommunicationEventListPage> Table => this.MaterialTable();

        public void Select(CommunicationEvent communicationEvent)
        {
            var row = this.Table.FindRow(communicationEvent);
            var cell = row.FindCell("subject");
            cell.Click();
        }
    }
}
