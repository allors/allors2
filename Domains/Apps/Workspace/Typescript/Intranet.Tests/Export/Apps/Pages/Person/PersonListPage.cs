namespace Pages.PersonTests
{
    using Allors.Domain;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PersonListPage : MainPage
    {
        public PersonListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input<PersonListPage> LastName => this.Input(formControlName: "lastName");

        public Anchor<PersonListPage> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public MaterialTable<PersonListPage> Table => this.MaterialTable();

        public PersonOverviewPage Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("name");
            cell.Click();

            return new PersonOverviewPage(this.Driver);
        }
    }
}
