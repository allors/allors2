namespace src.allors.material.custom.relations.people
{
    using src.allors.material.custom.relations.people.person;
    using Allors.Domain;
    using Components;

    public partial class PeopleComponent
    {
        public MatTable Table => new MatTable(this.Driver);

        public PersonOverviewComponent Select(Person person)
        {
            var row = this.Table.FindRow(person);
            var cell = row.FindCell("firstName");
            cell.Click();

            return new PersonOverviewComponent(this.Driver);
        }
    }
}
