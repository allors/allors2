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

        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Organisation.Name);

        public MaterialInput TaxNumber => new MaterialInput(this.Driver, roleType: M.Organisation.TaxNumber);

        public MaterialSingleSelect LegalForm => new MaterialSingleSelect(this.Driver, roleType: M.Organisation.LegalForm);

        public MaterialSingleSelect Locale => new MaterialSingleSelect(this.Driver, roleType: M.Organisation.Locale);

        public MaterialMultipleSelect IndustryClassifications => new MaterialMultipleSelect(this.Driver, roleType: M.Organisation.IndustryClassifications);

        public MaterialMultipleSelect CustomClassifications => new MaterialMultipleSelect(this.Driver, roleType: M.Organisation.CustomClassifications);

        public MaterialSlideToggle IsManufacturer => new MaterialSlideToggle(this.Driver, roleType: M.Organisation.IsManufacturer);

        public MaterialSlideToggle IsInternalOrganisation => new MaterialSlideToggle(this.Driver, roleType: M.Organisation.IsInternalOrganisation);

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.Organisation.Comment);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));

    }
}
