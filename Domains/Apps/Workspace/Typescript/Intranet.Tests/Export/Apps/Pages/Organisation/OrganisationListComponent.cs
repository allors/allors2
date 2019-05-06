using Angular.Html;
using OpenQA.Selenium;
using src.allors.material.apps.objects.person.list;

namespace src.allors.material.apps.objects.organisation.list
{
    using Allors.Domain;
    using Pages.OrganisationTests;

    public partial class OrganisationListComponent 
    {
        public Anchor<OrganisationListComponent> AddNew => this.Anchor(By.CssSelector("[mat-fab]"));

        public OrganisationOverviewPage Select(Organisation organisation)
        {
            var row = this.Table.FindRow(organisation);
            var cell = row.FindCell("name");
            cell.Click();

            return new OrganisationOverviewPage(this.Driver);
        }
    }
}
