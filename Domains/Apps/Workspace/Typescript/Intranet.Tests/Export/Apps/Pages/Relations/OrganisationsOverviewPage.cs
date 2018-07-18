namespace Intranet.Pages.Relations
{
    using Intranet.Tests;

    using OpenQA.Selenium;

    public class OrganisationsOverviewPage : MainPage
    {
        public OrganisationsOverviewPage(IWebDriver driver)
            : base(driver)
        {
        }

        public Input Name => new Input(this.Driver, formControlName: "name");

        public Anchor AddNew => new Anchor(this.Driver, By.LinkText("Add New"));
    }
}
