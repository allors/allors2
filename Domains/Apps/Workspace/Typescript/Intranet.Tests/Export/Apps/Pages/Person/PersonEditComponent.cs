using src.app.main;

namespace Pages.PersonTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class PersonEditComponent : MainComponent
    {
        public PersonEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatSelect<PersonEditComponent> Salutation => this.MatSelect(roleType: M.Person.Salutation);

        public MatSelect<PersonEditComponent> Gender => this.MatSelect(roleType: M.Person.Gender);

        public MatSelect<PersonEditComponent> Locale => this.MatSelect(roleType: M.Person.Locale);

        public MatInput<PersonEditComponent> FirstName => this.MatInput(roleType: M.Person.FirstName);

        public MatInput<PersonEditComponent> MiddleName => this.MatInput(roleType: M.Person.MiddleName);

        public MatInput<PersonEditComponent> LastName => this.MatInput(roleType: M.Person.LastName);

        public MatInput<PersonEditComponent> Function => this.MatInput(roleType: M.Person.Function);

        public MatTextarea<PersonEditComponent> Comment => this.MatTextarea(roleType: M.Person.Comment);

        public Button<PersonEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
