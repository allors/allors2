namespace Intranet.Pages.Relations
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PersonOverviewPage : MainPage
    {
        public PersonOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Button EditButton => new Button(this.Driver, By.XPath("//button/span[contains(text(), 'Edit')]"));

        public PersonPage Edit()
        {
            this.EditButton.Click();
            return new PersonPage(this.Driver);
        }
    }
}
