namespace Pages.Relations
{
    using OpenQA.Selenium;

    using Angular.Html;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button<PersonOverviewPage> EditButton => this.Button(By.XPath("//button/span[contains(text(), 'Edit')]"));

        public PersonEditPage Edit()
        {
            this.EditButton.Click();
            return new PersonEditPage(this.Driver);
        }
    }
}
