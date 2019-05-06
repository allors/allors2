
namespace src.allors.material.apps.objects.person.list
{
    using Pages.PersonTests;
    using Allors.Domain;
    using Angular.Html;
    using OpenQA.Selenium;

    public partial class PersonListComponent
    {
        public Anchor<PersonListComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public PersonOverviewPage Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("name");
            cell.Click();

            return new PersonOverviewPage(this.Driver);
        }
    }
}
