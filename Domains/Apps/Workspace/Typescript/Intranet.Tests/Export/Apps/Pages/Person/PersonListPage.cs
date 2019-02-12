namespace Pages.PersonTests
{
    using Allors.Domain;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    using Tests.PersonTests;

    public class PersonListPage : MainPage
    {
        public PersonListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input LastName => new Input(this.Driver, formControlName: "lastName");

        public Anchor AddNew => new Anchor(this.Driver, By.CssSelector("[mat-fab]"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public PersonOverviewPage Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("name");
            cell.Click();

            return new PersonOverviewPage(this.Driver);
        }
    }
}
