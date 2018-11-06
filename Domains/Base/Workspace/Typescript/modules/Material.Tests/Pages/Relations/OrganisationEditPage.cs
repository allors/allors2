namespace Tests.Material.Pages.Relations
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class OrganisationEditPage : MainPage
    {
        public OrganisationEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput Name => new MaterialInput(this.Driver, roleType: M.Organisation.Name);
        
        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
