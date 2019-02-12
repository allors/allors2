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

        public Input Name => new Input(this.Driver, formControlName: "Name");

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public OrganisationOverviewPage Select(Organisation organisation)
        {
            var row = this.Table.FindRow(organisation);
            var cell = row.FindCell("name");
            cell.Click();

            return new OrganisationOverviewPage(this.Driver);
        }
    }
}
