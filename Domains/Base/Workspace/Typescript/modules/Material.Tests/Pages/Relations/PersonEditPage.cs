namespace Tests.Material.Pages.Relations
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Tests.Components.Html;
    using Tests.Components.Material;

    public class PersonEditPage : MainPage
    {
        public PersonEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput FirstName => new MaterialInput(this.Driver, roleType: M.Person.FirstName);

        public MaterialInput MiddleName => new MaterialInput(this.Driver, roleType: M.Person.MiddleName);

        public MaterialInput LastName => new MaterialInput(this.Driver, roleType: M.Person.LastName);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
