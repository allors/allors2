namespace Pages.SerialisedItemTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SerialisedItemEditPage : MainPage
    {
        public SerialisedItemEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSingleSelect Salutation => new MaterialSingleSelect(this.Driver, roleType: M.Person.Salutation);

        public MaterialSingleSelect Gender => new MaterialSingleSelect(this.Driver, roleType: M.Person.Gender);

        public MaterialSingleSelect Locale => new MaterialSingleSelect(this.Driver, roleType: M.Person.Locale);

        public MaterialInput FirstName => new MaterialInput(this.Driver, roleType: M.Person.FirstName);

        public MaterialInput MiddleName => new MaterialInput(this.Driver, roleType: M.Person.MiddleName);

        public MaterialInput LastName => new MaterialInput(this.Driver, roleType: M.Person.LastName);

        public MaterialInput Function => new MaterialInput(this.Driver, roleType: M.Person.Function);

        public MaterialTextArea Comment => new MaterialTextArea(this.Driver, roleType: M.Person.Comment);

        public Button Save => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
