using src.allors.material.apps.objects.person.overview;

namespace src.allors.material.apps.objects.person.list
{
    using Allors.Domain;

    public partial class PersonListComponent
    {
        public PersonOverviewComponent Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("name");
            cell.Click();

            return new PersonOverviewComponent(this.Driver);
        }
    }
}
