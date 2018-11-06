namespace Tests.Intranet.Relations
{
    using Allors.Meta;

    using Tests.Intranet;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class OrganisationPage : MainPage
    {
        public OrganisationPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Organisation.Name);

        public MaterialInput TaxNumber => new MaterialInput(this.Driver, roleType: M.Organisation.TaxNumber);

        public MaterialSlideToggle IsManufacturer => new MaterialSlideToggle(this.Driver, roleType: M.Organisation.IsManufacturer);

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.Organisation.Comment);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
