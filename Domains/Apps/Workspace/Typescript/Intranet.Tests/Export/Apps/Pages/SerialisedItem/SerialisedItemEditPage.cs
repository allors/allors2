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

        public MaterialSelect<SerialisedItemEditPage> Salutation => this.MaterialSelect(roleType: M.Person.Salutation);

        public MaterialSelect<SerialisedItemEditPage> Gender => this.MaterialSelect(roleType: M.Person.Gender);

        public MaterialSelect<SerialisedItemEditPage> Locale => this.MaterialSelect(roleType: M.Person.Locale);

        public MaterialInput<SerialisedItemEditPage> FirstName => this.MaterialInput(roleType: M.Person.FirstName);

        public MaterialInput<SerialisedItemEditPage> MiddleName => this.MaterialInput(roleType: M.Person.MiddleName);

        public MaterialInput<SerialisedItemEditPage> LastName => this.MaterialInput(roleType: M.Person.LastName);

        public MaterialInput<SerialisedItemEditPage> Function => this.MaterialInput(roleType: M.Person.Function);

        public MaterialTextArea<SerialisedItemEditPage> Comment => this.MaterialTextArea(roleType: M.Person.Comment);

        public Button<SerialisedItemEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
