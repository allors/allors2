namespace src.allors.material.custom.relations.people
{
    using src.allors.material.custom.relations.people.person;
    using Allors.Domain;
    using Angular.Material;

    public partial class PeopleComponent
    {
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
