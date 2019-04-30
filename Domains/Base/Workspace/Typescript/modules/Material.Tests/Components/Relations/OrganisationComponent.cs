namespace src.allors.material.custom.relations.organisations.organisation
{
    using OpenQA.Selenium;

    using Angular.Html;

    public partial class OrganisationComponent
    {
        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
