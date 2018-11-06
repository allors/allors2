namespace Tests.Material.Pages.Relations
{
    using OpenQA.Selenium;

    using Tests.Components.Html;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public PersonEditPage Edit()
        {
            this.EditButton.Click();
            return new PersonEditPage(this.Driver);
        }
    }
}
