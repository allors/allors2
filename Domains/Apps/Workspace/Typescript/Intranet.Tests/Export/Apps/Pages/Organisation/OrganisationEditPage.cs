namespace Pages.OrganisationTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class OrganisationEditPage : MainPage
    {
        public OrganisationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput<OrganisationEditPage> Name => this.MaterialInput(roleType: M.Organisation.Name);

        public MaterialInput<OrganisationEditPage> TaxNumber => this.MaterialInput(roleType: M.Organisation.TaxNumber);

        public MaterialSingleSelect<OrganisationEditPage> LegalForm => this.MaterialSingleSelect(roleType: M.Organisation.LegalForm);

        public MaterialSingleSelect<OrganisationEditPage> Locale => this.MaterialSingleSelect(roleType: M.Organisation.Locale);

        public MaterialMultipleSelect<OrganisationEditPage> IndustryClassifications => this.MaterialMultipleSelect(roleType: M.Organisation.IndustryClassifications);

        public MaterialMultipleSelect<OrganisationEditPage> CustomClassifications => this.MaterialMultipleSelect(roleType: M.Organisation.CustomClassifications);

        public MaterialSlideToggle<OrganisationEditPage> IsManufacturer => this.MaterialSlideToggle(roleType: M.Organisation.IsManufacturer);

        public MaterialSlideToggle<OrganisationEditPage> IsInternalOrganisation => this.MaterialSlideToggle(roleType: M.Organisation.IsInternalOrganisation);

        public MaterialTextArea<OrganisationEditPage> Comment => this.MaterialTextArea(roleType: M.Organisation.Comment);

        public Button<OrganisationEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));

    }
}
