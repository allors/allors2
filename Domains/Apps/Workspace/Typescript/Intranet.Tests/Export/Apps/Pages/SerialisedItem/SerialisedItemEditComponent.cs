using src.app.main;

namespace Pages.SerialisedItemTests
{
    using Allors.Meta;
    using Components;

    using OpenQA.Selenium;

    using Pages.ApplicationTests;

    public class SerialisedItemEditComponent : MainComponent
    {
        public SerialisedItemEditComponent(IWebDriver driver)
            : base(driver)
        {
        }

        public MatSelect<SerialisedItemEditComponent> Salutation => this.MatSelect(roleType: M.Person.Salutation);

        public MatSelect<SerialisedItemEditComponent> Gender => this.MatSelect(roleType: M.Person.Gender);

        public MatSelect<SerialisedItemEditComponent> Locale => this.MatSelect(roleType: M.Person.Locale);

        public MatInput<SerialisedItemEditComponent> FirstName => this.MatInput(roleType: M.Person.FirstName);

        public MatInput<SerialisedItemEditComponent> MiddleName => this.MatInput(roleType: M.Person.MiddleName);

        public MatInput<SerialisedItemEditComponent> LastName => this.MatInput(roleType: M.Person.LastName);

        public MatInput<SerialisedItemEditComponent> Function => this.MatInput(roleType: M.Person.Function);

        public MatTextarea<SerialisedItemEditComponent> Comment => this.MatTextarea(roleType: M.Person.Comment);

        public Button<SerialisedItemEditComponent> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
