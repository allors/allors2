namespace src.allors.material.custom.relations.organisations.organisation
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

    public partial class OrganisationComponent 
    {
        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Organisation.Name);
        
        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
