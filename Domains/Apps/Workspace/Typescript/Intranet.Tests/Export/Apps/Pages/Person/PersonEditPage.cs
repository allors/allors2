namespace Pages.PersonTests
{
    using Allors.Meta;

    using Angular.Html;
    using Angular.Material;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PersonEditPage : MainPage
    {
        public PersonEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialSingleSelect<PersonEditPage> Salutation => this.MaterialSingleSelect(roleType: M.Person.Salutation);

        public MaterialSingleSelect<PersonEditPage> Gender => this.MaterialSingleSelect(roleType: M.Person.Gender);

        public MaterialSingleSelect<PersonEditPage> Locale => this.MaterialSingleSelect(roleType: M.Person.Locale);

        public MaterialInput<PersonEditPage> FirstName => this.MaterialInput(roleType: M.Person.FirstName);

        public MaterialInput<PersonEditPage> MiddleName => this.MaterialInput(roleType: M.Person.MiddleName);

        public MaterialInput<PersonEditPage> LastName => this.MaterialInput(roleType: M.Person.LastName);

        public MaterialInput<PersonEditPage> Function => this.MaterialInput(roleType: M.Person.Function);

        public MaterialTextArea<PersonEditPage> Comment => this.MaterialTextArea(roleType: M.Person.Comment);

        public Button<PersonEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
