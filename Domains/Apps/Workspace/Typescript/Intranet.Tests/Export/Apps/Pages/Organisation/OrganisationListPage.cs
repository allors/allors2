namespace Pages.OrganisationTests
{
    using Allors.Domain;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class OrganisationListPage : MainPage
    {
        public OrganisationListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<OrganisationListPage> Name => this.Input(formControlName: "Name");

        public Anchor<OrganisationListPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public MaterialTable<OrganisationListPage> Table => this.MaterialTable();

        public OrganisationOverviewPage Select(Organisation organisation)
        {
            var row = this.Table.FindRow(organisation);
            var cell = row.FindCell("name");
            cell.Click();

            return new OrganisationOverviewPage(this.Driver);
        }
    }
}
