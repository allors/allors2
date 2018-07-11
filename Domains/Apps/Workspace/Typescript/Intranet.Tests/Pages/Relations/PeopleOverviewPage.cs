namespace Intranet.Pages
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class PeopleOverviewPage : Page
    {
        public PeopleOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input LastName => new Input(this.Driver, formControlName: "lastName");
    }
}
