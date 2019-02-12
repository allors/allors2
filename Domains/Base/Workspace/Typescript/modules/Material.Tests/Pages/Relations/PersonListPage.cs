namespace Pages.Relations
{
    using Allors.Domain;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

    public class PersonListPage : MainPage
    {
        public PersonListPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input LastName => new Input(this.Driver, formControlName: "lastName");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public PersonOverviewPage Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();

            return new PersonOverviewPage(this.Driver);
        }
    }
}
