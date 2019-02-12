namespace Pages.Relations
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

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
