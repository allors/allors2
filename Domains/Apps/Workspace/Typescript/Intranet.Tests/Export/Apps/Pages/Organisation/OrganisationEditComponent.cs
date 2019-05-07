using src.app.main;

namespace Pages.OrganisationTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class OrganisationEditComponent : MainComponent
    {
        public OrganisationEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatInput<OrganisationEditComponent> Name => this.MatInput(roleType: M.Organisation.Name);

        public MatInput<OrganisationEditComponent> TaxNumber => this.MatInput(roleType: M.Organisation.TaxNumber);

        public MatSelect<OrganisationEditComponent> LegalForm => this.MatSelect(roleType: M.Organisation.LegalForm);

        public MatSelect<OrganisationEditComponent> Locale => this.MatSelect(roleType: M.Organisation.Locale);

        public MatSelect<OrganisationEditComponent> IndustryClassifications => this.MatSelect(roleType: M.Organisation.IndustryClassifications);

        public MatSelect<OrganisationEditComponent> CustomClassifications => this.MatSelect(roleType: M.Organisation.CustomClassifications);

        public MatSlidetoggle<OrganisationEditComponent> IsManufacturer => this.MatSlidetoggle(roleType: M.Organisation.IsManufacturer);

        public MatSlidetoggle<OrganisationEditComponent> IsInternalOrganisation => this.MatSlidetoggle(roleType: M.Organisation.IsInternalOrganisation);

        public MatTextarea<OrganisationEditComponent> Comment => this.MatTextarea(roleType: M.Organisation.Comment);

        public Button<OrganisationEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

    }
}
