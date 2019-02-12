namespace Pages.Relations
{
    using Allors.Meta;

    using OpenQA.Selenium;

    using Angular.Html;
    using Angular.Material;

    public class PersonEditPage : MainPage
    {
        public PersonEditPage(IWebDriver driver)
            : base(driver)
        {
        }

        public MaterialInput<PersonEditPage> FirstName => this.MaterialInput(M.Person.FirstName);

        public MaterialInput<PersonEditPage> MiddleName => this.MaterialInput(M.Person.MiddleName);

        public MaterialInput<PersonEditPage> LastName => this.MaterialInput(M.Person.LastName);

        public Button<PersonEditPage> Save => this.Button(By.XPath("//button/span[contains(text(), 'SAVE')]"));
    }
}
