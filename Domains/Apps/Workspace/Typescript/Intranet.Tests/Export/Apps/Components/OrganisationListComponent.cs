using src.allors.material.apps.objects.organisation.overview;

namespace src.allors.material.apps.objects.organisation.list
{
    using Allors.Domain;

    public partial class OrganisationListComponent 
    {
        public OrganisationOverviewComponent Select(Organisation organisation)
        {
            var row = this.Table.FindRow(organisation);
            var cell = row.FindCell("name");
            cell.Click();

            return new OrganisationOverviewComponent(this.Driver);
        }
    }
}
