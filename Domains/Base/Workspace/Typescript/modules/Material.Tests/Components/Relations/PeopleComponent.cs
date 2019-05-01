namespace src.allors.material.custom.relations.people
{
    using src.allors.material.custom.relations.people.person;
    using Allors.Domain;
    using OpenQA.Selenium;
    using Angular.Html;
    using Angular.Material;

    public partial class PeopleComponent
    {
        public Input LastName => new Input(this.Driver, By.CssSelector($"input[formcontrolname='lastName']"));

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));

        public MaterialTable Table => new MaterialTable(this.Driver);

        public PersonOverviewComponent Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();

            return new PersonOverviewComponent(this.Driver);
        }
    }
}
